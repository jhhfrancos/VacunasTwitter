import { Component, OnInit } from '@angular/core';
import { analytics } from 'googleapis/build/src/apis/analytics';
import { chdir } from 'process';
import { BehaviorSubject } from 'rxjs';
import { Subscription } from 'rxjs/Subscription';
import { LdaService } from './lda.component.service';
import { HierarchyDatum } from '../commons/models/tree-chart/HierarchyDatum.interface'
import { HierarchyNode } from 'd3';

@Component({
  selector: 'app-lda',
  templateUrl: './lda.component.html',
  styleUrls: ['./lda.component.css']
})
export class LdaComponent implements OnInit {


  //Contains all subscription for the component
  private subscriptions: Subscription[] = [];
  loading$ = this.ldaService.loadingSubject$.asObservable();
  loadingLdaTree$ = this.ldaService.loadingLdaSubject$.asObservable();
  loadingTrainningLda$ = this.ldaService.loadingTrainnginLdaSubject$.asObservable();
  
  public ldaTree = {} as HierarchyDatum;
  private ldaTreeCopy : HierarchyDatum;
  public ldas: String[];
  public ldaTrainning: boolean;
  public text : string;

  constructor(private ldaService: LdaService) { }

  ngOnInit() {
    this.subscriptions.push(this.ldaService.payload$
      .subscribe(result => {
        this.ldas = result;
      }));

    this.subscriptions.push(this.ldaService.ldaSubject$
      .subscribe(result => {
        //this.ldaTree = result;
        if (!this.ldaTreeCopy)
          this.ldaTree = this.createHierarchicalDatum(result);
        else{
          var newNode = this.createHierarchicalDatum(result);
          this.winNode.name = newNode.name;
          this.winNode.children = newNode.children;
          this.winNode._id = newNode._id;
          this.ldaTree = this.ldaTreeCopy;
        }
      }));

    this.subscriptions.push(this.ldaService.trainnginLdaSubject$
      .subscribe(result => {
        this.ldaTrainning = result;
      }));

    this.ldaService.getTestResultLDA("7");
    this.ldaService.getTestLDA("covid");
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }
  inputChange(event){
    this.ldaTreeCopy = null;
    this.ldaService.getTestLDA(event.target.value);
  }
  TrainningLDA(): void {
    this.ldaService.getTraingLDA("1000000");
  }
  reeplaceNewLineStrings(text: string): string {
    return text.replace(/\n/gi, "<br>") || "";
  }

  private createHierarchicalDatum(hierarchyDatum): HierarchyDatum {
    var ldaTreeMid = {} as HierarchyDatum;

    hierarchyDatum.forEach(element => {
      ldaTreeMid.name = element.text;
      ldaTreeMid._id = generateID();
      var listTopics = element.topics.split(/\n+/);
      var childrens = new Array<HierarchyDatum>();
      listTopics.slice(1).forEach(element => {
        var child = {} as HierarchyDatum;
        var splitting: Array<string>;
        var nameValue = element.split("=>");
        child.name = nameValue[0];
        splitting = nameValue[1].match(/\S+/g);
        child.children = splitting.map<HierarchyDatum>((childs) => {
          var split = childs.split(/\[|\]/);
          return tranformeHierarchical(split[0], +split[1]);
        });
        childrens.push(child);
      });
      ldaTreeMid.children = childrens;

    });
    return ldaTreeMid;
  }

  clickItem(item: any) {
    this.ldaTreeCopy = JSON.parse(JSON.stringify(this.ldaTree));
    this.searchNode(item.data._id, this.ldaTreeCopy);
    this.ldaService.getTestLDA(item.data.name);
    
  }

  public winNode: HierarchyDatum = null;
  searchNode(key: string, node: HierarchyDatum): void {
    if (node._id == key) {
      this.winNode = node;
    } else {
      node.children?.forEach(element => {
        this.searchNode(key, element);
      });
    }
  }
}

function tranformeHierarchical(name: string, value: number): HierarchyDatum {
  return {
    name: name,
    value: value,
    _id: generateID()
  }
}

function generateID(): string {
  // Math.random should be unique because of its seeding algorithm.
  // Convert it to base 36 (numbers + letters), and grab the first 9 characters
  // after the decimal.
  return '_' + Math.random().toString(36).substr(2, 9);
};

