import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'movimentacao' },
  {
    path: 'movimentacao',
    loadChildren: () => import('./movimentacao/movimentacao.module').then(m => m.MovimentacaoModule)
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
