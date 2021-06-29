import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import * as d3 from 'd3';

@Component({
  selector: 'app-scatter-svg',
  templateUrl: './scatter-svg.component.html',
  styleUrls: ['./scatter-svg.component.css']
})
export class ScatterSvgComponent implements OnInit {

  @Input() data: any;
  @ViewChild('tsne', { static: true }) private chartContainer: ElementRef;
  /*private data = [
    { "Framework": "Vue", "Stars": "166443", "Released": "2014" },
    { "Framework": "React", "Stars": "150793", "Released": "2013" },
    { "Framework": "Angular", "Stars": "62342", "Released": "2016" },
    { "Framework": "Backbone", "Stars": "27647", "Released": "2010" },
    { "Framework": "Ember", "Stars": "21471", "Released": "2011" },
  ];*/

  private svg;
  private minX: number = 0;
  private maxY: number = 0;
  private minY: number = 0;
  private maxX: number = 0;
  private selectedNode: any;
  private margin = 50;
  private width = 750 - (this.margin * 2);
  private height = 400 - (this.margin * 2);
  private _fillScale;          // D3 scale for text color

  constructor() { }

  ngOnInit(): void {
    let points: Array<any> = [];
    for (let item = 0; item < this.data.mapXY.length; item++) {
      const element = this.data.mapXY[item];
      const elementTarget = this.data.targets[item];
      const documents = this.data.documents[item];
      if (Number(element[0]) > this.maxX) this.maxX = element[0];
      if (Number(element[0]) < this.minX) this.minX = element[0];
      if (Number(element[1]) > this.maxY) this.maxY = element[1];
      if (Number(element[1]) < this.minY) this.minY = element[1];
      points.push({ "XY": element, "Label": elementTarget, "Document": documents });
    }
    this.data.scatters = points;
    this._fillScale = d3.scaleOrdinal(d3.schemeCategory10);
    //Make ths graphics
    this.createSvg();
    this.drawPlot();
  }

  private createSvg(): void {
    let element: any = this.chartContainer.nativeElement;

    this.width = element.offsetWidth;
    this.height = element.offsetHeight;

    this.svg = d3.select("figure#scatter")
      .append("svg")
      .attr("width", this.width)
      .attr("height", this.height)
      .append("g");
      //.attr("transform", "translate(" + this.margin + "," + this.margin + ")");
  }

  private drawPlot(): void {
    // Add X axis
    const x = d3.scaleLinear()
      .domain([this.minX, this.maxX])
      .range([0, this.width]);
    this.svg.append("g")
      .attr("transform", "translate(0," + this.height + ")")
      .call(d3.axisBottom(x).tickFormat(d3.format("d")));

    // Add Y axis
    const y = d3.scaleLinear()
      .domain([this.minY, this.maxY])
      .range([this.height, 0]);
    this.svg.append("g")
      .call(d3.axisLeft(y));



    // Add dots
    const dots = this.svg.append('g');

    // Define the div for the tooltip
    var div = d3.select("body").append('div')
      .attr('class', 'tooltip')
      .style('opacity', 0)
      .style('background', "rgb(37, 38, 39)")
      .style('color', "white");

    dots.selectAll("dot")
      .data(this.data.scatters)
      .enter()
      .append("circle")
      .attr("cx", d => x(d.XY[0]))
      .attr("cy", d => y(d.XY[1]))
      .attr("r", 7)
      .attr('cursor', "pointer")
      .style("fill", (d, i) => {
        return this._fillScale(d.Label);
      }).on("click", (d, i) => {
        if (this.selectedNode != i) {
          div.transition()
            .duration(200)
            .style('opacity', .9);
          div.html("Tweet: " + i.Document.document + "</br> Temas: </br>" + getArrayToString(i.Document.topicDescriptions))
            .style('left', (d.pageX + 14) + 'px')
            .style('top', (d.pageY - 14) + 'px');
          this.svg.selectAll('circle').style('opacity', (node: any) =>
            (node.Label != i.Label) ? 0.1 : 1
          );
          this.selectedNode = i;
        } else {
          div.transition()
            .duration(500)
            .style("opacity", 0);
          div.html("");
          this.svg.selectAll('circle').style('opacity', 1);
          this.selectedNode = null;
        }
      });



    // Add labels
    /*dots.selectAll("text")
      .data(this.data.scatters)
      .enter()
      .append("text")
      //.text(d => d.Label)
      .attr("x", d => x(d.XY[0]))
      .attr("y", d => y(d.XY[1]))*/
  }


}

function getArrayToString(arreglo: any[]): string {
  let returnString = "";
  for (let index = 0; index < arreglo.length; index++) {
    const element = arreglo[index];
    returnString = `${returnString} ${element.score} => ${Object.keys(element.tokens)} </br>`;
  }
  return returnString;
}

