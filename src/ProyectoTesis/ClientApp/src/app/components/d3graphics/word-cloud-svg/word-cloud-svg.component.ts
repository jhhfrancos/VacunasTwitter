import { Component, ElementRef, Input, OnInit } from '@angular/core';
import * as D3 from 'd3';
import * as d3Cloud from "d3-cloud";


@Component({
  selector: 'app-word-cloud-svg',
  templateUrl: './word-cloud-svg.component.html',
  styleUrls: ['./word-cloud-svg.component.css']
})
export class WordCloudSvgComponent implements OnInit {


  @Input() inputData: Array<{word: string, size: number}>;

  //private data: any[];
  private data: any[];
  //private d3: any;

  private _host;              // D3 object referencing host DOM object
  private _svg;               // SVG in which we will print our chart
  private _margin: {          // Space between the svg borders and the actual chart graphic
    top: number,
    right: number,
    bottom: number,
    left: number
  };
  private _width: number;      // Component width
  private _height: number;     // Component height
  private _htmlElement: HTMLElement; // Host HTMLElement
  private _minCount: number;   // Minimum word count
  private _maxCount: number;   // Maximum word count
  private _fontScale;          // D3 scale for font size
  private _fillScale;          // D3 scale for text color


  constructor(private _element: ElementRef) {
    this._htmlElement = this._element.nativeElement;
    this._host = D3.select(this._element.nativeElement);
  }
  ngOnInit(): void {
    this.DataCleansing();

    /*this.data = [
      "Hello", "world", "normally", "you", "want", "more", "words",
      "than", "this"]
      .map((d) => ({ word: d, size: 10 + Math.random() * 90, test: "haha" }));
*/

    this._setup();
    this._buildSVG();
    this._populate();
  }

  private DataCleansing(): void {
    /*this.words = this.source.split(/[\s.]+/g)
      .map(w => w.replace(/^[“‘"\-—()\[\]{}]+/g, ""))
      .map(w => w.replace(/[;:.!?()\[\]{},"'’”\-—]+$/g, ""))
      .map(w => w.replace(/['’]s$/g, ""))
      .map(w => w.substring(0, 30))
      .map(w => w.toLowerCase())
      .filter(w => w && !this.stopwords.has(w))

    this.words.filter(w => /\W/.test(w))

    this.data = D3.rollups(this.words, group => group.length, w => w)
      .sort(([, a], [, b]) => D3.descending(a, b))
      .slice(0, 250)
      .map(([word, size]) => ({ word: word, size: size })); //10 + Math.random() * 90}

    */

    this.data = this.inputData.map((t) => ({word: t.word, size: 10 + t.size * 90}));//map(({word, size}) => ({ word: word, size: 10 + size * 90 }));
    //this.data = this.inputData;
    console.log("data" + this.data);

    //this.d3 = Object.assign(await require("d3@6"), {cloud: await require("d3-cloud@1")})
  }


  private _setup() {
    this._margin = {
      top: 10,
      right: 10,
      bottom: 10,
      left: 10
    };
    this._width = ((this._htmlElement.parentElement.clientWidth == 0)
      ? 300
      : this._htmlElement.parentElement.clientWidth) - this._margin.left - this._margin.right;
    if (this._width < 100) {
      this._width = 100;
    }
    this._height = this._width * 0.75 - this._margin.top - this._margin.bottom;

    this._minCount = D3.min(this.data, d => d.size);
    this._maxCount = D3.max(this.data, d => d.size);

    let minFontSize: number = 10;
    let maxFontSize: number = 25;
    this._fontScale = D3.scalePow()
      .domain([this._minCount, this._maxCount])
      .range([minFontSize, maxFontSize]);
    this._fillScale = D3.scaleOrdinal(D3.schemeCategory10);
  }

  private _buildSVG() {
    this._host.html('');
    this._svg = this._host
      .append('svg')
      .attr('width', this._width + this._margin.left + this._margin.right)
      .attr('height', this._height + this._margin.top + this._margin.bottom)
      .append('g')
      .attr('transform', 'translate(' + ~~(this._width / 2) + ',' + ~~(this._height / 2) + ')');
  }

  private _populate() {
    let fontFace: string = 'Roboto';
    let fontWeight: string = 'normal';
    let spiralType: string = 'archimedean'; //archimedean, rectangular

    d3Cloud()
      .size([this._width, this._height])
      .words(this.data)
      .rotate(() => 0)
      .font(fontFace)
      .fontWeight(fontWeight)
      .fontSize(d => this._fontScale(d.size))
      .spiral(spiralType)
      .on('end', () => { //"word" and "end"
        this._drawWordCloud(this.data);
      })
      .start();
  }

  private _drawWordCloud(words) {
    this._svg
      .selectAll('text')
      .data(words)
      .enter()
      .append('text')
      .style('font-size', d => d.size + 'px')
      .style('fill', (d, i) => {
        return this._fillScale(i);
      })
      .attr('text-anchor', 'middle')
      .attr('transform', d => 'translate(' + [d.x, d.y] + ')rotate(' + d.rotate + ')')
      .attr('class', 'word-cloud')
      .text(d => {
        return d.word;
      });
  }
}
