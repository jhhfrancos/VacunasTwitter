import { Component, ElementRef, Input, OnInit } from '@angular/core';
import * as D3 from 'd3';
import * as d3Cloud from "d3-cloud";


@Component({
  selector: 'app-word-cloud-svg',
  templateUrl: './word-cloud-svg.component.html',
  styleUrls: ['./word-cloud-svg.component.css']
})
export class WordCloudSvgComponent implements OnInit {


  //@Input() config: WordCloudConfig;

  private source: string = "@RomagueraTeresa: @cubadebatecu Ahora, lo 1ro es demostrar en esta etapa q la vacuna no induce efectos adversos, q afecten a las persona @cubadebatecu Ahora, lo 1ro es demostrar en esta etapa q la vacuna no induce efectos adversos, q afecten a las personas q se la pongan y en 2da instancia, evaluar elementos que permitan analizar su eficacia. \nArriba mi tierra, #Cuba Va\n#Soberana01 https://t.co/Roj9BRHCEK #QuedateEnCasa Programas de salud\n\nSolicita a domicilio en #IPSCajasan:\n\nConsulta médica\n\nTerapias físicas y especiales\n\nVacunación NO PA\n\nVenta de leches de fórmula y complementos nutricionales\n\nInfo: Bucaramanga: 300 302 5431/301 200 6732\nBarrancabermeja: 318 433 6904 https://t.co/JRMUxyZhXB ";
  private words: string[];
  private stopwords = new Set("actualmente, acuerdo, adelante, ademas, además, adrede, afirmó, agregó, ahi, ahora, ahí, al, algo, alguna, algunas, alguno, algunos, algún, alli, allí, alrededor, ambos, ampleamos, antano, antaño, ante, anterior, antes, apenas, aproximadamente, aquel, aquella, aquellas, aquello, aquellos, aqui, aquél, aquélla, aquéllas, aquéllos, aquí, arriba, arribaabajo, aseguró, asi, así, atras, aun, aunque, ayer, añadió, aún, bajo, bastante, bien, breve, buen, buena, buenas, bueno, buenos, cada, casi, cerca, cierta, ciertas, cierto, ciertos, cinco, claro, comentó, como, con, conmigo, conocer, conseguimos, conseguir, considera, consideró, consigo, consigue, consiguen, consigues, contigo, contra, cosas, creo, cual, cuales, cualquier, cuando, cuanta, cuantas, cuanto, cuantos, cuatro, cuenta, cuál, cuáles, cuándo, cuánta, cuántas, cuánto, cuántos, cómo, da, dado, dan, dar, de, debajo, debe, deben, debido, decir, dejó, del, delante, demasiado, demás, dentro, deprisa, desde, despacio, despues, después, detras, detrás, dia, dias, dice, dicen, dicho, dieron, diferente, diferentes, dijeron, dijo, dio, donde, dos, durante, día, días, dónde, ejemplo, el, ella, ellas, ello, ellos, embargo, empleais, emplean, emplear, empleas, empleo, en, encima, encuentra, enfrente, enseguida, entonces, entre, era, eramos, eran, eras, eres, es, esa, esas, ese, eso, esos, esta, estaba, estaban, estado, estados, estais, estamos, estan, estar, estará, estas, este, esto, estos, estoy, estuvo, está, están, ex, excepto, existe, existen, explicó, expresó, él, ésa, ésas, ése, ésos, ésta, éstas, éste, éstos, fin, final, fue, fuera, fueron, fui, fuimos, general, gran, grandes, gueno, ha, haber, habia, habla, hablan, habrá, había, habían, hace, haceis, hacemos, hacen, hacer, hacerlo, haces, hacia, haciendo, hago, han, hasta, hay, haya, he, hecho, hemos, hicieron, hizo, horas, hoy, hubo, igual, incluso, indicó, informo, informó, intenta, intentais, intentamos, intentan, intentar, intentas, intento, ir, junto, la, lado, largo, las, le, lejos, les, llegó, lleva, llevar, lo, los, luego, lugar, mal, manera, manifestó, mas, mayor, me, mediante, medio, mejor, mencionó, menos, menudo, mi, mia, mias, mientras, mio, mios, mis, misma, mismas, mismo, mismos, modo, momento, mucha, muchas, mucho, muchos, muy, más, mí, mía, mías, mío, míos, nada, nadie, ni, ninguna, ningunas, ninguno, ningunos, ningún, no, nos, nosotras, nosotros, nuestra, nuestras, nuestro, nuestros, nueva, nuevas, nuevo, nuevos, nunca, ocho, os, otra, otras, otro, otros, pais, para, parece, parte, partir, pasada, pasado, paìs, peor, pero, pesar, poca, pocas, poco, pocos, podeis, podemos, poder, podria, podriais, podriamos, podrian, podrias, podrá, podrán, podría, podrían, poner, por, porque, posible, primer, primera, primero, primeros, principalmente, pronto, propia, propias, propio, propios, proximo, próximo, próximos, pudo, pueda, puede, pueden, puedo, pues, qeu, que, quedó, queremos, quien, quienes, quiere, quiza, quizas, quizá, quizás, quién, quiénes, qué, raras, realizado, realizar, realizó, repente, respecto, sabe, sabeis, sabemos, saben, saber, sabes, salvo, se, sea, sean, segun, segunda, segundo, según, seis, ser, sera, será, serán, sería, señaló, si, sido, siempre, siendo, siete, sigue, siguiente, sin, sino, sobre, sois, sola, solamente, solas, solo, solos, somos, son, soy, soyos, su, supuesto, sus, suya, suyas, suyo, sé, sí, sólo, tal, tambien, también, tampoco, tan, tanto, tarde, te, temprano, tendrá, tendrán, teneis, tenemos, tener, tenga, tengo, tenido, tenía, tercera, ti, tiempo, tiene, tienen, toda, todas, todavia, todavía, todo, todos, total, trabaja, trabajais, trabajamos, trabajan, trabajar, trabajas, trabajo, tras, trata, través, tres, tu, tus, tuvo, tuya, tuyas, tuyo, tuyos, tú, ultimo, un, una, unas, uno, unos, usa, usais, usamos, usan, usar, usas, uso, usted, ustedes, última, últimas, último, últimos, va, vais, valor, vamos, van, varias, varios, vaya, veces, ver, verdad, verdadera, verdadero, vez, vosotras, vosotros, voy, vuestra, vuestras, vuestro, vuestros, ya, yo".split(","));
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
      .map(([word, size]) => ({ word: word, size: size })); //10 + Math.random() * 90}

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

    let minFontSize: number = 20;
    let maxFontSize: number = 70;
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