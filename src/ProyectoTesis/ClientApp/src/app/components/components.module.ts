import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { FooterComponent } from './footer/footer.component';
import { NavbarComponent } from './navbar/navbar.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { SettingsComponent } from './settings/settings.component';
import { BarSvgComponent } from './d3graphics/bar-svg/bar-svg.component';
import { WordCloudSvgComponent } from './d3graphics/word-cloud-svg/word-cloud-svg.component';
import { CollapsibleTreeSvgComponent } from './d3graphics/collapsible-tree-svg/collapsible-tree-svg.component';


@NgModule({
  imports: [
    CommonModule,
    RouterModule,
  ],
  declarations: [
    FooterComponent,
    NavbarComponent,
    SidebarComponent,
    SettingsComponent,
    BarSvgComponent,
    WordCloudSvgComponent,
    CollapsibleTreeSvgComponent
  ],
  exports: [
    FooterComponent,
    NavbarComponent,
    SidebarComponent,
    SettingsComponent,
    BarSvgComponent,
    WordCloudSvgComponent,
    CollapsibleTreeSvgComponent
  ]
})
export class ComponentsModule { }
