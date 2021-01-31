import { Component, OnInit, ViewEncapsulation } from '@angular/core';

import * as d3 from 'd3';

interface Node {
  id: string;
  group: number;
  index: number;
  x: number;
  y: number;
  vx: number;
  xy: number;
}

interface Link {
  source: string;
  target: string;
  value: number;
}

interface Graph {
  nodes: Node[];
  links: Link[];
}

const data = {
  "nodes": [
    {"id": "Myriel", "group": 1},
    {"id": "Napoleon", "group": 1},
    {"id": "Mlle.Baptistine", "group": 3},
    {"id": "Mme.Magloire", "group": 2},
    {"id": "CountessdeLo", "group": 3}
    
  ],
  "links": [
    {"source": "Myriel", "target": "Napoleon", "value": 1},
    {"source": "Mlle.Baptistine", "target": "Mme.Magloire", "value": 8},
    {"source": "Mme.Magloire", "target": "Myriel", "value": 10},
  ]
};

@Component({
  selector: 'app-force-directed-svg',
  templateUrl: './force-directed-svg.component.html',
  styleUrls: ['./force-directed-svg.component.css']
})
export class ForceDirectedSvgComponent implements OnInit {

  ngOnInit() {
    console.log('D3.js version:', d3['version']);

    const svg = d3.select('#directed');
    const width = +svg.attr('width');
    const height = +svg.attr('height');

    const color = d3.scaleOrdinal(d3.schemeSet3);

    const simulation = d3.forceSimulation()
      .force('link', d3.forceLink().id((d: any) => d.id))
      .force('charge', d3.forceManyBody())
      .force('center', d3.forceCenter(width / 2, height / 2));

    

      const nodes: Node[] = [];
      const links: Link[] = [];

      data.nodes.forEach((d) => {
        nodes.push(<Node>d);
      });

      data.links.forEach((d) => {
        links.push(<Link>d);
      });
      const graph: Graph = <Graph>{ nodes, links };

      const link = svg.append('g')
        .attr('class', 'linksForce')
        .selectAll('line')
        .data(graph.links)
        .enter()
        .append('line')
        .attr('stroke', "#999")
        .attr('stroke-opacity', 0.6)
        .attr('stroke-width', (d: any) => Math.sqrt(d.value));

      const node = svg.append('g')
        .attr('class', 'nodesForce')
        .selectAll('circle')
        .data(graph.nodes)
        .enter()
        .append('circle')
        .attr('r', 5)
        .attr('stroke', "#fff")
        .attr('stroke-width', "1.5px")
        .attr('cursor', "pointer")
        .attr('fill', (d: any) => color(d.group));


      svg.selectAll('circle').call(d3.drag()
        .on('start', dragstarted)
        .on('drag', dragged)
        .on('end', dragended)
      );

      node.append('title')
        .text((d) => d.id);

      simulation
        .nodes(graph.nodes)
        .on('tick', ticked);

      simulation.force<d3.ForceLink<any, any>>('link')
        .links(graph.links);

      function ticked() {
        link
          .attr('x1', function(d: any) { return d.source.x; })
          .attr('y1', function(d: any) { return d.source.y; })
          .attr('x2', function(d: any) { return d.target.x; })
          .attr('y2', function(d: any) { return d.target.y; });

        node
          .attr('cx', function(d: any) { return d.x; })
          .attr('cy', function(d: any) { return d.y; });
      }
    

    function dragstarted(event, d) {
      //if (!d3.event.active) { simulation.alphaTarget(0.3).restart(); }
      if(!event.active) simulation.alphaTarget(0.3).restart();
      d.fx = d.x;
      d.fy = d.y;
    }

    function dragged(event, d) {
      d.fx = event.x;//d3.event.x;
      d.fy = event.y;//d3.event.y;
    }

    function dragended(event, d) {
      //if (!d3.event.active) { simulation.alphaTarget(0); }
      if(!event.active) simulation.alphaTarget(0);
      d.fx = null;
      d.fy = null;
    }
  }

}
