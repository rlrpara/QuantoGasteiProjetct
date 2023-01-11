export interface Movimentacao {
  codigo: number;
  tipo: number;
  descricao: string;
  mes?: number;
  ano?: number;
  dataVencimento?: Date;
  dataPagamento?: Date;
  parcela?: number;
  totalParcelas?: number;
  valor?: number;
}
