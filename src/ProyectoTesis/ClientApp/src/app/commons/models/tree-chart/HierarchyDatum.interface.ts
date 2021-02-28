export interface HierarchyDatum {
    name: string;
    value?: number;
    children?: Array<HierarchyDatum>;
    _id?: string;
    x0?: number;
    y0?: number;
  }