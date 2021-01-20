import { Component, ElementRef, Input, KeyValueDiffers, OnInit } from '@angular/core';
import * as D3 from 'd3';
import * as d3Cloud from "d3-cloud";


@Component({
  selector: 'app-word-cloud-svg',
  templateUrl: './word-cloud-svg.component.html',
  styleUrls: ['./word-cloud-svg.component.css']
})
export class WordCloudSvgComponent implements OnInit {


  //@Input() config: WordCloudConfig;

  private source: string = " of honoring this sacred obligation, America has given the Negro people a bad check, a check which has come back marked “insufficient funds.” But we refuse to believe that the bank of justice is bankrupt. We refuse to believe that there are insufficient funds in the great vaults of opportunity of this nation. So we have come to cash this check — a check that will give";
  private words: string[];
  private stopwords = new Set("i,me,my,myself,we,us,our,ours,ourselves,you,your,yours,yourself,yourselves,he,him,his,himself,she,her,hers,herself,it,its,itself,they,them,their,theirs,themselves,what,which,who,whom,whose,this,that,these,those,am,is,are,was,were,be,been,being,have,has,had,having,do,does,did,doing,will,would,should,can,could,ought,i'm,you're,he's,she's,it's,we're,they're,i've,you've,we've,they've,i'd,you'd,he'd,she'd,we'd,they'd,i'll,you'll,he'll,she'll,we'll,they'll,isn't,aren't,wasn't,weren't,hasn't,haven't,hadn't,doesn't,don't,didn't,won't,wouldn't,shan't,shouldn't,can't,cannot,couldn't,mustn't,let's,that's,who's,what's,here's,there's,when's,where's,why's,how's,a,an,the,and,but,if,or,because,as,until,while,of,at,by,for,with,about,against,between,into,through,during,before,after,above,below,to,from,up,upon,down,in,out,on,off,over,under,again,further,then,once,here,there,when,where,why,how,all,any,both,each,few,more,most,other,some,such,no,nor,not,only,own,same,so,than,too,very,say,says,said,shall".split(","));
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
  private _objDiffer;

  constructor(private _element: ElementRef, private _keyValueDiffers: KeyValueDiffers) {
    this._htmlElement = this._element.nativeElement;
    this._host = D3.select(this._element.nativeElement);
    this._objDiffer = this._keyValueDiffers.find([]).create();
  }
  ngOnInit(): void {
    this.DataCleansing();
    //this.CreateSvg();
    //this.drawBars(this.data);
    /*this.data =[{ text: "hola", size: 10, word:"hola1" },
    { text: "hola", size: 20, word:"hola2" },
    { text: "hola", size: 30, word:"hola3" }];*/
    /*
    let svg: any = D3.select("figure#word")
      .append("svg")
      .attr("width", 850)
      .attr("height", 350);
    d3Cloud().size([800, 300])
      .words(datos);*/



    this._setup();
    this._buildSVG();
    this._populate();
  }

  private DataCleansing(): void {
    this.words = this.source.split(/[\s.]+/g)
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
      .map(([word, size]) => ({ word, size }));

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

    let minFontSize: number = 18;
    let maxFontSize: number = 96;
    this._fontScale = D3.scaleLinear()
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
    let spiralType: string = 'rectangular';

    d3Cloud()
      .size([this._width, this._height])
      .words(this.data)
      .rotate(() => 0)
      .font(fontFace)
      .fontWeight(fontWeight)
      .fontSize(d => this._fontScale(d.size))
      .spiral(spiralType)
      .on('end', () => {
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

  /*private source: string = " of honoring this sacred obligation, America has given the Negro people a bad check, a check which has come back marked “insufficient funds.” But we refuse to believe that the bank of justice is bankrupt. We refuse to believe that there are insufficient funds in the great vaults of opportunity of this nation. So we have come to cash this check — a check that will give";
  private words: string[];
  private stopwords = new Set("i,me,my,myself,we,us,our,ours,ourselves,you,your,yours,yourself,yourselves,he,him,his,himself,she,her,hers,herself,it,its,itself,they,them,their,theirs,themselves,what,which,who,whom,whose,this,that,these,those,am,is,are,was,were,be,been,being,have,has,had,having,do,does,did,doing,will,would,should,can,could,ought,i'm,you're,he's,she's,it's,we're,they're,i've,you've,we've,they've,i'd,you'd,he'd,she'd,we'd,they'd,i'll,you'll,he'll,she'll,we'll,they'll,isn't,aren't,wasn't,weren't,hasn't,haven't,hadn't,doesn't,don't,didn't,won't,wouldn't,shan't,shouldn't,can't,cannot,couldn't,mustn't,let's,that's,who's,what's,here's,there's,when's,where's,why's,how's,a,an,the,and,but,if,or,because,as,until,while,of,at,by,for,with,about,against,between,into,through,during,before,after,above,below,to,from,up,upon,down,in,out,on,off,over,under,again,further,then,once,here,there,when,where,why,how,all,any,both,each,few,more,most,other,some,such,no,nor,not,only,own,same,so,than,too,very,say,says,said,shall".split(","));
  private data: any[];
  private svg;
  private margin = 50;
  private width = 750 - (this.margin * 2);
  private height = 400 - (this.margin * 2);
  private d3: any;
 
  constructor() { }
 
  ngOnInit(): void {
    this.DataCleansing();
    //this.CreateSvg();
    //this.drawBars(this.data);
  }
 
  private CreateSvg(): void {
    this.svg = D3.select("figure#bar")
      .append("svg")
      .attr("width", this.width + (this.margin * 2))
      .attr("height", this.height + (this.margin * 2))
      .append("g")
      .attr("transform", "translate(" + this.margin + "," + this.margin + ")");
  }
 
  private DataCleansing(): void{
    this.words = this.source.split(/[\s.]+/g)
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
      .map(([text, value]) => ({ text, value }));
 
      this.d3 = Object.assign(await require("d3@6"), {cloud: await require("d3-cloud@1")})
  }
 
  private drawBars(data: any[]): void {
    
  }*/

}
