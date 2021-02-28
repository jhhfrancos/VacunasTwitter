import { identifierModuleUrl } from '@angular/compiler';
import { Component, ViewChild, ElementRef, OnInit, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import * as d3 from "d3";
import { HierarchyNode, TreeLayout } from 'd3';
import { BehaviorSubject, from, Observable } from 'rxjs';
import { threadId } from 'worker_threads';
import { HierarchyDatum } from '../../../commons/models/tree-chart/HierarchyDatum.interface'


/*
const data: HierarchyDatum = {
  "name": "vacunas",
  "children": [
    {
      "name": "países",
      "children": [
        {
          "name": "turismo",
        },
        {
          "name": "vacunación",
          "children": [
            { "name": "aborto", "value": 3534 },
            { "name": "dinero", "value": 5731 },
            { "name": "trabajadores", "value": 7840 },
            { "name": "justicia", "value": 5914 }
          ]
        }
      ]
    },
    {
      "name": "mito",
      "children": [
        { "name": "europa", "value": 17010 },
        { "name": "riqueza", "value": 5842 },
        { "name": "hacefríoctm", "value": 1041 },
        { "name": "sostenible", "value": 5176 }
      ]
    },
    {
      "name": "COVID-19",
      "children": [
        { "name": "brasil", "value": 1759 },
        { "name": "necesidad", "value": 2165 },
        { "name": "presa", "value": 586 },
        { "name": "fenómeno", "value": 3331 },
        { "name": "vacuna", "value": 772 }
      ]
    },
    {
      "name": "seguridad",
      "children": [
        { "name": "vaccines", "value": 8833 },
        { "name": "vacunas", "value": 1732 },
        { "name": "dermatologists", "value": 3623 },
        { "name": "adolescencia", "value": 10066 }
      ]
    }
  ]
};*/
@Component({
  selector: 'app-collapsible-tree-svg',
  templateUrl: './collapsible-tree-svg.component.html',
  styleUrls: ['./collapsible-tree-svg.component.css']
})
export class CollapsibleTreeSvgComponent implements OnInit, OnChanges {

  title = 'd3tree';
  @ViewChild('chart', { static: true }) private chartContainer: ElementRef;
  @Input() data: HierarchyDatum;//BehaviorSubject<HierarchyDatum>;
  @Output() clickRequest = new EventEmitter<any>();

  root: any;
  tree: TreeLayout<HierarchyDatum>;

  //data: HierarchyDatum;
  //datumSubscription: Observable<HierarchyDatum>;

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
  verticalSeparationBetweenNodes: number = 30;
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
    //this.datumSubscription = this.datumSubject.asObservable();
    // this.datumSubscription.subscribe(data => {
    //   this.data = data;
    //   this.buildData(this.data);
    // });
    //this.renderTreeChart();
  }
  ngOnChanges(changes: SimpleChanges){
    //this.renderTreeChart();
    this.data = changes.data.currentValue
    let element: any = this.chartContainer.nativeElement;
    d3.select(element).select("svg").remove();
    //this.buildData(this.data);
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
      .separation((a, b) => { return a.parent == b.parent ? 2 : 4 })
      ;

    this.heightSubscription.subscribe(data => {

      let gElement = this.svg.node();
      let hei = gElement.getBoundingClientRect().height;
      data = hei > data ? hei : data;

      let element: any = this.chartContainer.nativeElement;
      d3.select(element).select('svg')
        // .attr("viewBox", `0,${-data/2},${element.offsetWidth},${data/2}`)
        .attr('height', data + this.margin.top + this.margin.bottom)
        .select("g")
        .attr('transform', 'translate(' + this.margin.left + ',' + data / 2 + ')');
    });

    this.buildData(this.data);

    //Collapse after the second level
    //this.root.children.forEach(collapse);

    function collapse(d) {
      if (d.children) {
        d._children = d.children;
        d._children.forEach(collapse);
        d.children = null;
      }
    }

  }

  buildData(source) {
    // Assigns parent, children, height, depth
    this.root = d3.hierarchy<HierarchyDatum>(source, (d) => { return d.children; });
    this.root.x0 = this.height / 2;
    this.root.y0 = 10;

    this.updateChart(this.root);
  }

  click = (d) => {
    console.log("click emit" + this.data);
    if (!(d._children || d.children)) {
      this.clickRequest.emit(d);
      return;
    }
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
      .attr('r', 5)
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
