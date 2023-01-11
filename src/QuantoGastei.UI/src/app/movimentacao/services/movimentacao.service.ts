import { Movimentacao } from './../model/movimentacao';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class MovimentacaoService {



  constructor(private http: HttpClient) { }

  ObterTodos(): Movimentacao[] {
    return [
      {
        codigo: 1,
        descricao: 'Energia Elétrica',
        dataVencimento: new Date('2023-03-10T00:00:00.000Z'),
        parcela: 1,
        totalParcelas: 1,
        tipo: 1,
        valor: 318.58
      }
    ]
  }
}
