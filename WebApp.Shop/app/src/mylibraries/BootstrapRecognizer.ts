export type BootstrapClass = "xxl" | "lg" | "md" | "sm" | "xs";
export class BTSizes {
  constructor(bootstrapClass: BootstrapClass, phoneOrTablet: boolean, media: string) {
    this.bootstrapClass = bootstrapClass;
    this.media = window.matchMedia(media);
    this.phoneOrTablet = phoneOrTablet;
  }
  bootstrapClass: BootstrapClass;
  media: MediaQueryList;
  phoneOrTablet: boolean;
}
type BTEvent = (size: BTSizes) => void;
export class BootstrapRecognizer {
  events: BTEvent[] = [];
  dimensions = [
    new BTSizes("xxl", false, "(min-width: 1200px)"),
    new BTSizes("lg", false, "(max-width: 1200px) and (min-width: 992px)"),
    new BTSizes("md", true, "(max-width: 992px) and (min-width: 767px)"),
    new BTSizes("sm", true, "(max-width: 767px) and (min-width: 576px)"),
    new BTSizes("xs", true, "(max-width: 576px)")
  ];
  constructor() {
    window.addEventListener("resize", this.involve.bind(this));
  }
  involve() {
    for (let i of this.dimensions) {
      if (i.media.matches) {
        console.log(i.phoneOrTablet)
        this.events.forEach(x => x(i));
        break;
      }
    }
  }
}
