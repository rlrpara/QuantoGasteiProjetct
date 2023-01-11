import { Component } from '@angular/core';
import { MovimentacaoService } from '../services/movimentacao.service';

import { Movimentacao } from './../model/movimentacao';

@Component({
  selector: 'app-movimentacao',
  templateUrl: './movimentacao.component.html',
  styleUrls: ['./movimentacao.component.scss']
})
export class MovimentacaoComponent {

  movimentacoes: Movimentacao[];

  constructor(private movimentacaoService: MovimentacaoService) {
    this.movimentacoes = this.movimentacaoService.ObterTodos();
  }

  displayColumns = ['codigo', 'descricao', 'dataVencimento', 'dataPagamento', 'parcela', 'totalParcelas', 'valor'];

}
