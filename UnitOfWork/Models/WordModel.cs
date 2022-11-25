using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.UnitOfWork.Models
{
    public class WordModel
    {
        public string Name { get; set; }
        public string Celler { get; set; }
        public static IEnumerable<WordModel> Parse(List<string> words)
        {
            foreach (var word in words)
            {
                var model = word.Split(".");
                yield return new WordModel
                {
                    Celler = model[0],
                    Name = model[1],
                };
            }
        }
    }
}
