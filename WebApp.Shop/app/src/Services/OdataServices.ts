import DataTransmitter from './DataTransmitter'
import { DataTransmitterOptions } from './DataTransmitter'

type wherePredicateType<TSource> = (item: TSource) => boolean;

interface IQueryable<TSource> {
    Where(predicate: wherePredicateType<TSource>): IQueryable<TSource>;
    Execute(options?: DataTransmitterOptions): void;
}

class QueryableOption {
    url: string;
    filterSequences: Array<Array<string>>;
    constructor(url: string) {
        this.url = url;
        this.filterSequences = [];
    }
}

class Queryable<TSource> implements IQueryable<TSource>
{
    qOption: QueryableOption;
    constructor(qOption: QueryableOption) {
        this.qOption = qOption;
    }
    Where(predicate: wherePredicateType<TSource>): IQueryable<TSource> {
        this.qOption.filterSequences.push(compile(predicate.toString()));
        return this;
    }

    async Execute(options?: DataTransmitterOptions) {
        let url = this.qOption.url + (this.qOption.url.includes("?") ? "" : "?");
        if (!['?', '&'].includes(url[url.length - 1])) url += "&";
        url += (this.qOption.filterSequences.length === 0) ? "" : `$filter=${execute(this.qOption.filterSequences)}`;
        return (await DataTransmitter.GetRequest<any>(url, options)).value;
    }
}

export class OdataSetProtocol<TSource> extends Queryable<TSource>
{
    constructor(url: string) {
        super(new QueryableOption(url));
    }
}

function execute(filterSequences: Array<Array<string>>) {
    let result = "";
    filterSequences.forEach((sequence1, i1) => {
        result += "("
        sequence1.forEach((sequence2, i2) => {
            if (sequence2 === "===")
                result += 'eq'
            else
                result += sequence2;
            if (i2 !== sequence1.length - 1)
                result += ' ';
        });
        result += ")"
    })
    return result.replace(/\"/g, `'`,);
}

function compile(spr: string) {
    let index = 0;
    let currentLetter = '';
    let br = true;
    let isStarted = false;
    let sequence: Array<string> = [];
    let params: Array<string> = [];
    let sequenceIsStarted = false;
    function flush(add = true) {
        if (add)
            sequence.push(currentLetter);
        currentLetter = "";
    }
    while (br) {
        if (index >= spr.length) {
            flush();
            break;
        }
        let currentChar = spr[index++];
        if (currentChar === ' ')
            continue;
        if (currentChar === '=') {
            if (!sequenceIsStarted) {
                sequenceIsStarted = true;
                flush();
            }
            if (currentLetter === "==") {
                currentLetter += currentChar;
                flush();
                continue;
            }
        }
        else if (currentChar === ">") {
            if (currentLetter === "=") {
                flush(false);
                sequenceIsStarted = false;
                params.push(sequence.pop());
                continue;
            }
        }
        
        currentLetter += currentChar;
    }
    let spParam = params[0] + ".";
    sequence = sequence.map(x => x.startsWith(spParam) ? x.replace(spParam, "") : x);
    return sequence;
}
