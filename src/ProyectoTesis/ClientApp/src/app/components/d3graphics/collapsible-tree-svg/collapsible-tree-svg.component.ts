import { Component, ViewChild, ElementRef, OnInit } from '@angular/core';
import * as d3 from "d3";
import { HierarchyNode, TreeLayout } from 'd3';
import { BehaviorSubject, Observable } from 'rxjs';

interface HierarchyDatum {
  name: string;
  value?: number;
  children?: Array<HierarchyDatum>;
  x0?: number;
  y0?: number;
}

const data: HierarchyDatum = {
  "name": "flare",
  "children": [
    {
      "name": "analytics",
      "children": [
        {
          "name": "cluster",
          "children": [
            { "name": "AgglomerativeCluster", "value": 3938 },
            { "name": "CommunityStructure", "value": 3812 },
            { "name": "HierarchicalCluster", "value": 6714 },
            { "name": "MergeEdge", "value": 743 }
          ]
        },
        {
          "name": "graph",
          "children": [
            { "name": "BetweennessCentrality", "value": 3534 },
            { "name": "LinkDistance", "value": 5731 },
            { "name": "MaxFlowMinCut", "value": 7840 },
            { "name": "ShortestPaths", "value": 5914 },
            { "name": "SpanningTree", "value": 3416 }
          ]
        },
        {
          "name": "optimization",
          "children": [
            { "name": "AspectRatioBanker", "value": 7074 }
          ]
        }
      ]
    },
    {
      "name": "animate",
      "children": [
        { "name": "Easing", "value": 17010 },
        { "name": "FunctionSequence", "value": 5842 },
        {
          "name": "interpolate",
          "children": [
            { "name": "ArrayInterpolator", "value": 1983 },
            { "name": "ColorInterpolator", "value": 2047 },
            { "name": "DateInterpolator", "value": 1375 },
            { "name": "Interpolator", "value": 8746 },
            { "name": "MatrixInterpolator", "value": 2202 },
            { "name": "NumberInterpolator", "value": 1382 },
            { "name": "ObjectInterpolator", "value": 1629 },
            { "name": "PointInterpolator", "value": 1675 },
            { "name": "RectangleInterpolator", "value": 2042 }
          ]
        },
        { "name": "ISchedulable", "value": 1041 },
        { "name": "Parallel", "value": 5176 },
        { "name": "Pause", "value": 449 },
        { "name": "Scheduler", "value": 5593 },
        { "name": "Sequence", "value": 5534 },
        { "name": "Transition", "value": 9201 },
        { "name": "Transitioner", "value": 19975 },
        { "name": "TransitionEvent", "value": 1116 },
        { "name": "Tween", "value": 6006 }
      ]
    },
    {
      "name": "data",
      "children": [
        {
          "name": "converters",
          "children": [
            { "name": "Converters", "value": 721 },
            { "name": "DelimitedTextConverter", "value": 4294 },
            { "name": "GraphMLConverter", "value": 9800 },
            { "name": "IDataConverter", "value": 1314 },
            { "name": "JSONConverter", "value": 2220 }
          ]
        },
        { "name": "DataField", "value": 1759 },
        { "name": "DataSchema", "value": 2165 },
        { "name": "DataSet", "value": 586 },
        { "name": "DataSource", "value": 3331 },
        { "name": "DataTable", "value": 772 },
        { "name": "DataUtil", "value": 3322 }
      ]
    },
    {
      "name": "display",
      "children": [
        { "name": "DirtySprite", "value": 8833 },
        { "name": "LineSprite", "value": 1732 },
        { "name": "RectSprite", "value": 3623 },
        { "name": "TextSprite", "value": 10066 }
      ]
    },
    {
      "name": "flex",
      "children": [
        { "name": "FlareVis", "value": 4116 }
      ]
    },
    {
      "name": "physics",
      "children": [
        { "name": "DragForce", "value": 1082 },
        { "name": "GravityForce", "value": 1336 },
        { "name": "IForce", "value": 319 },
        { "name": "NBodyForce", "value": 10498 },
        { "name": "Particle", "value": 2822 },
        { "name": "Simulation", "value": 9983 },
        { "name": "Spring", "value": 2213 },
        { "name": "SpringForce", "value": 1681 }
      ]
    },
    {
      "name": "query",
      "children": [
        { "name": "AggregateExpression", "value": 1616 },
        { "name": "And", "value": 1027 },
        { "name": "Arithmetic", "value": 3891 },
        { "name": "Average", "value": 891 },
        { "name": "BinaryExpression", "value": 2893 },
        { "name": "Comparison", "value": 5103 },
        { "name": "CompositeExpression", "value": 3677 },
        { "name": "Count", "value": 781 },
        { "name": "DateUtil", "value": 4141 },
        { "name": "Distinct", "value": 933 },
        { "name": "Expression", "value": 5130 },
        { "name": "ExpressionIterator", "value": 3617 },
        { "name": "Fn", "value": 3240 },
        { "name": "If", "value": 2732 },
        { "name": "IsA", "value": 2039 },
        { "name": "Literal", "value": 1214 },
        { "name": "Match", "value": 3748 },
        { "name": "Maximum", "value": 843 },
        {
          "name": "methods",
          "children": [
            { "name": "add", "value": 593 },
            { "name": "and", "value": 330 },
            { "name": "average", "value": 287 },
            { "name": "count", "value": 277 },
            { "name": "distinct", "value": 292 },
            { "name": "div", "value": 595 },
            { "name": "eq", "value": 594 },
            { "name": "fn", "value": 460 },
            { "name": "gt", "value": 603 },
            { "name": "gte", "value": 625 },
            { "name": "iff", "value": 748 },
            { "name": "isa", "value": 461 },
            { "name": "lt", "value": 597 },
            { "name": "lte", "value": 619 },
            { "name": "max", "value": 283 },
            { "name": "min", "value": 283 },
            { "name": "mod", "value": 591 },
            { "name": "mul", "value": 603 },
            { "name": "neq", "value": 599 },
            { "name": "not", "value": 386 },
            { "name": "or", "value": 323 },
            { "name": "orderby", "value": 307 },
            { "name": "range", "value": 772 },
            { "name": "select", "value": 296 },
            { "name": "stddev", "value": 363 },
            { "name": "sub", "value": 600 },
            { "name": "sum", "value": 280 },
            { "name": "update", "value": 307 },
            { "name": "variance", "value": 335 },
            { "name": "where", "value": 299 },
            { "name": "xor", "value": 354 },
            { "name": "_", "value": 264 }
          ]
        },
        { "name": "Minimum", "value": 843 },
        { "name": "Not", "value": 1554 },
        { "name": "Or", "value": 970 },
        { "name": "Query", "value": 13896 },
        { "name": "Range", "value": 1594 },
        { "name": "StringUtil", "value": 4130 },
        { "name": "Sum", "value": 791 },
        { "name": "Variable", "value": 1124 },
        { "name": "Variance", "value": 1876 },
        { "name": "Xor", "value": 1101 }
      ]
    },
    {
      "name": "scale",
      "children": [
        { "name": "IScaleMap", "value": 2105 },
        { "name": "LinearScale", "value": 1316 },
        { "name": "LogScale", "value": 3151 },
        { "name": "OrdinalScale", "value": 3770 },
        { "name": "QuantileScale", "value": 2435 },
        { "name": "QuantitativeScale", "value": 4839 },
        { "name": "RootScale", "value": 1756 },
        { "name": "Scale", "value": 4268 },
        { "name": "ScaleType", "value": 1821 },
        { "name": "TimeScale", "value": 5833 }
      ]
    },
    {
      "name": "util",
      "children": [
        { "name": "Arrays", "value": 8258 },
        { "name": "Colors", "value": 10001 },
        { "name": "Dates", "value": 8217 },
        { "name": "Displays", "value": 12555 },
        { "name": "Filter", "value": 2324 },
        { "name": "Geometry", "value": 10993 },
        {
          "name": "heap",
          "children": [
            { "name": "FibonacciHeap", "value": 9354 },
            { "name": "HeapNode", "value": 1233 }
          ]
        },
        { "name": "IEvaluable", "value": 335 },
        { "name": "IPredicate", "value": 383 },
        { "name": "IValueProxy", "value": 874 },
        {
          "name": "math",
          "children": [
            { "name": "DenseMatrix", "value": 3165 },
            { "name": "IMatrix", "value": 2815 },
            { "name": "SparseMatrix", "value": 3366 }
          ]
        },
        { "name": "Maths", "value": 17705 },
        { "name": "Orientation", "value": 1486 },
        {
          "name": "palette",
          "children": [
            { "name": "ColorPalette", "value": 6367 },
            { "name": "Palette", "value": 1229 },
            { "name": "ShapePalette", "value": 2059 },
            { "name": "SizePalette", "value": 2291 }
          ]
        },
        { "name": "Property", "value": 5559 },
        { "name": "Shapes", "value": 19118 },
        { "name": "Sort", "value": 6887 },
        { "name": "Stats", "value": 6557 },
        { "name": "Strings", "value": 22026 }
      ]
    },
    {
      "name": "vis",
      "children": [
        {
          "name": "axis",
          "children": [
            { "name": "Axes", "value": 1302 },
            { "name": "Axis", "value": 24593 },
            { "name": "AxisGridLine", "value": 652 },
            { "name": "AxisLabel", "value": 636 },
            { "name": "CartesianAxes", "value": 6703 }
          ]
        },
        {
          "name": "controls",
          "children": [
            { "name": "AnchorControl", "value": 2138 },
            { "name": "ClickControl", "value": 3824 },
            { "name": "Control", "value": 1353 },
            { "name": "ControlList", "value": 4665 },
            { "name": "DragControl", "value": 2649 },
            { "name": "ExpandControl", "value": 2832 },
            { "name": "HoverControl", "value": 4896 },
            { "name": "IControl", "value": 763 },
            { "name": "PanZoomControl", "value": 5222 },
            { "name": "SelectionControl", "value": 7862 },
            { "name": "TooltipControl", "value": 8435 }
          ]
        },
        {
          "name": "data",
          "children": [
            { "name": "Data", "value": 20544 },
            { "name": "DataList", "value": 19788 },
            { "name": "DataSprite", "value": 10349 },
            { "name": "EdgeSprite", "value": 3301 },
            { "name": "NodeSprite", "value": 19382 },
            {
              "name": "render",
              "children": [
                { "name": "ArrowType", "value": 698 },
                { "name": "EdgeRenderer", "value": 5569 },
                { "name": "IRenderer", "value": 353 },
                { "name": "ShapeRenderer", "value": 2247 }
              ]
            },
            { "name": "ScaleBinding", "value": 11275 },
            { "name": "Tree", "value": 7147 },
            { "name": "TreeBuilder", "value": 9930 }
          ]
        },
        {
          "name": "events",
          "children": [
            { "name": "DataEvent", "value": 2313 },
            { "name": "SelectionEvent", "value": 1880 },
            { "name": "TooltipEvent", "value": 1701 },
            { "name": "VisualizationEvent", "value": 1117 }
          ]
        },
        {
          "name": "legend",
          "children": [
            { "name": "Legend", "value": 20859 },
            { "name": "LegendItem", "value": 4614 },
            { "name": "LegendRange", "value": 10530 }
          ]
        },
        {
          "name": "operator",
          "children": [
            {
              "name": "distortion",
              "children": [
                { "name": "BifocalDistortion", "value": 4461 },
                { "name": "Distortion", "value": 6314 },
                { "name": "FisheyeDistortion", "value": 3444 }
              ]
            },
            {
              "name": "encoder",
              "children": [
                { "name": "ColorEncoder", "value": 3179 },
                { "name": "Encoder", "value": 4060 },
                { "name": "PropertyEncoder", "value": 4138 },
                { "name": "ShapeEncoder", "value": 1690 },
                { "name": "SizeEncoder", "value": 1830 }
              ]
            },
            {
              "name": "filter",
              "children": [
                { "name": "FisheyeTreeFilter", "value": 5219 },
                { "name": "GraphDistanceFilter", "value": 3165 },
                { "name": "VisibilityFilter", "value": 3509 }
              ]
            },
            { "name": "IOperator", "value": 1286 },
            {
              "name": "label",
              "children": [
                { "name": "Labeler", "value": 9956 },
                { "name": "RadialLabeler", "value": 3899 },
                { "name": "StackedAreaLabeler", "value": 3202 }
              ]
            },
            {
              "name": "layout",
              "children": [
                { "name": "AxisLayout", "value": 6725 },
                { "name": "BundledEdgeRouter", "value": 3727 },
                { "name": "CircleLayout", "value": 9317 },
                { "name": "CirclePackingLayout", "value": 12003 },
                { "name": "DendrogramLayout", "value": 4853 },
                { "name": "ForceDirectedLayout", "value": 8411 },
                { "name": "IcicleTreeLayout", "value": 4864 },
                { "name": "IndentedTreeLayout", "value": 3174 },
                { "name": "Layout", "value": 7881 },
                { "name": "NodeLinkTreeLayout", "value": 12870 },
                { "name": "PieLayout", "value": 2728 },
                { "name": "RadialTreeLayout", "value": 12348 },
                { "name": "RandomLayout", "value": 870 },
                { "name": "StackedAreaLayout", "value": 9121 },
                { "name": "TreeMapLayout", "value": 9191 }
              ]
            },
            { "name": "Operator", "value": 2490 },
            { "name": "OperatorList", "value": 5248 },
            { "name": "OperatorSequence", "value": 4190 },
            { "name": "OperatorSwitch", "value": 2581 },
            { "name": "SortOperator", "value": 2023 }
          ]
        },
        { "name": "Visualization", "value": 16540 }
      ]
    }
  ]
}
  ;
@Component({
  selector: 'app-collapsible-tree-svg',
  templateUrl: './collapsible-tree-svg.component.html',
  styleUrls: ['./collapsible-tree-svg.component.css']
})
export class CollapsibleTreeSvgComponent implements OnInit {

  title = 'd3tree';
  @ViewChild('chart', { static: true }) private chartContainer: ElementRef;


  root: any;
  tree: TreeLayout<HierarchyDatum>;

  svg: any;

  treeData: any;

  height: number;
  bahaviorHeight: BehaviorSubject<number>;
  heightSubscription: Observable<number>;
  width: number;
  margin: any = { top: 20, bottom: 20, left: 50, right: 100 };
  duration: number = 0;
  nodeWidth: number = 5;
  nodeHeight: number = 5;
  nodeRadius: number = 5;
  horizontalSeparationBetweenNodes: number = 5;
  verticalSeparationBetweenNodes: number = 5;
  nodeTextDistanceY: string = "-5px";
  nodeTextDistanceX: number = 5;

  dragStarted: boolean;
  draggingNode: any;
  nodes: any[];
  selectedNodeByDrag: any;

  selectedNodeByClick: any;
  previousClickedDomNode: any;
  links: any;

  constructor() {
  }

  ngOnInit() {
    this.renderTreeChart();
  }

  renderTreeChart() {

    let element: any = this.chartContainer.nativeElement;

    this.width = element.offsetWidth - this.margin.left - this.margin.right;
    this.height = element.offsetHeight - this.margin.top - this.margin.bottom;
    this.bahaviorHeight = new BehaviorSubject<number>(this.height);
    this.heightSubscription = this.bahaviorHeight.asObservable();

    this.svg = d3.select(element).append('svg')
      // .attr("viewBox", `0,0,${element.offsetWidth},${element.offsetHeight}`)
      // .attr("preserveAspectRatio", "xMinYMin meet")
      .attr('width', element.offsetWidth)
      .attr('height', element.offsetHeight)
      .append("g")
      .attr('transform', 'translate(' + this.margin.left + ',' + this.height / 2 + ')');

    // declares a tree layout and assigns the size
    this.tree = d3.tree<HierarchyDatum>()
      .size([this.height, this.width])
      .nodeSize([this.nodeWidth + this.horizontalSeparationBetweenNodes, this.nodeHeight + this.verticalSeparationBetweenNodes])
      //.separation((a, b) => { return a.parent == b.parent ? 2 : 4 })
      ;

    // Assigns parent, children, height, depth
    this.root = d3.hierarchy<HierarchyDatum>(data, (d) => { return d.children; });
    this.root.x0 = this.height / 2;
    this.root.y0 = 10;

    this.heightSubscription.subscribe(data => {

      let gElement = this.svg.node();
      let hei = gElement.getBoundingClientRect().height;
      data = hei > data ? hei : data;

      let element: any = this.chartContainer.nativeElement;
      d3.select(element).select('svg')
        // .attr("viewBox", `0,${-data/2},${element.offsetWidth},${data/2}`)
        .attr('height', data + this.margin.top + this.margin.bottom )
        .select("g")
        .attr('transform', 'translate(' + this.margin.left + ',' + data / 2 + ')');
    });

    //Collapse after the second level
    this.root.children.forEach(collapse);

    this.updateChart(this.root);

    function collapse(d) {
      if (d.children) {
        d._children = d.children;
        d._children.forEach(collapse);
        d.children = null;
      }
    }

  }

  click = (d) => {
    console.log('click');
    if (d.children) {
      d._children = d.children;
      d.children = null;
    } else {
      d.children = d._children;
      d._children = null;
    }
    this.updateChart(d);
  }

  updateChart(source) {
    let i = 0;
    console.log("datos" + source);


    this.treeData = this.tree(this.root);
    this.nodes = this.treeData.descendants();
    this.links = this.treeData.descendants().slice(1);
    this.nodes.forEach((d) => {
      d.y = d.depth * (this.width / this.nodes[this.nodes.length - 1].depth);
      d.x = d.x;
    });

    let node = this.svg.selectAll('g.node')
      .data(this.nodes, (d) => { return d.id || (d.id = ++i); });

    let nodeEnter = node.enter().append('g')
      .attr('class', 'node')
      .attr('transform', (d) => {
        return 'translate(' + source.y0 + ',' + source.x0 + ')';
      })
      .on('click', (d, i) => this.click(i));

    nodeEnter.append('circle')
      .attr('class', 'node')
      .attr('r', 2.5)
      .style('fill', (d) => {
        return d._children ? "#555" : "#999";
      })
      .attr('cursor', 'pointer');

    nodeEnter.append('text')
      .attr('dy', '.35em')
      .attr('x', (d) => {
        return d.children || d._children ? -6 : 6;
      })
      .attr('text-anchor', (d) => {
        return d.children || d._children ? 'end' : 'start';
      })
      .style('font', '12px sans-serif')
      .text((d) => { return d.data.name; });

    let nodeUpdate = node.merge(nodeEnter).transition()
      .duration(this.duration)
      .attr('transform', (d) => {

        return 'translate(' + d.y + ',' + d.x + ')';
      })
      .attr("fill-opacity", 1)
      .attr("stroke-opacity", 1);

    /*nodeUpdate.select('circle.node')
      .attr('r', 2.5)
      .style('stroke-width', '1px')
      .style('stroke', 'steelblue')
      .style('fill', (d) => {
        return d._children ? 'lightsteelblue' : '#fff';
      })
      .attr('cursor', 'pointer');*/

    let nodeExit = node.exit().transition().remove()
      .duration(this.duration)
      .attr('transform', (d) => {
        //this.bahaviorHeight.next(this.height);
        return 'translate(' + source.y + ',' + source.x + ')';
      })
      .attr("fill-opacity", 0)
      .attr("stroke-opacity", 0)
      .on("end", (d) =>
        this.bahaviorHeight.next(this.height)
      );

    /*nodeExit.select('circle')
      .attr('r', 1e-6);

    nodeExit.select('text')
      .style('fill-opacity', 1e-6);*/

    let link = this.svg.selectAll('path.link')
      .data(this.links, (d) => { return d.id; });

    let linkEnter = link.enter().insert('path', 'g')
      .attr('class', 'link')
      .style('fill', 'none')
      .style('stroke', '#ccc')
      .style('stroke-width', '2px')
      .attr('d', function (d) {
        let o = { x: source.x0, y: source.y0 };
        return diagonal(o, o);
      });

    let linkUpdate = link.merge(linkEnter);

    linkUpdate.transition()
      .duration(this.duration)
      .attr('d', (d) => {
        //this.bahaviorHeight.next(this.height);
        return diagonal(d, d.parent);
      });

    link.exit().transition().remove()
      .duration(this.duration)
      .attr('d', function (d) {
        //this.bahaviorHeight.next(this.height);
        let o = { x: source.x, y: source.y };
        return diagonal(o, o);
      });

    this.nodes.forEach((d) => {
      d.x0 = d.x;
      d.y0 = d.y;
    });


    function diagonal(s, d) {
      let path = `M ${s.y} ${s.x}
                  C ${(s.y + d.y) / 2} ${s.x},
                  ${(s.y + d.y) / 2} ${d.x},
                  ${d.y} ${d.x}`;
      return path;
    }

  }

}
