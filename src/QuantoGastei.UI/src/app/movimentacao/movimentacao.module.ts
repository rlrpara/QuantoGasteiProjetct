import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MovimentacaoRoutingModule } from './movimentacao-routing.module';
import { MovimentacaoComponent } from './movimentacao/movimentacao.component';


@NgModule({
  declarations: [
    MovimentacaoComponent
  ],
  imports: [
    CommonModule,
    MovimentacaoRoutingModule
  ]
})
export class MovimentacaoModule { }
