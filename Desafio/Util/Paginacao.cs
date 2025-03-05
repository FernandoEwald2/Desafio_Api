using System;
using System.Collections.Generic;

namespace Desafio.Api.Util
{
    public class Paginacao<T>
    {
        public List<T> Items { get; set; }
        public int TotalDeRegistros { get; set; }
        public int TotalDePaginas { get; set; }
        public int PaginaAtual { get; set; }
        public int QuantidadePorPagina { get; set; }
        public Paginacao(List<T> items, int totalRecords, int currentPage, int pageSize)
        {
            Items = items;
            TotalDeRegistros = totalRecords;
            QuantidadePorPagina = pageSize;
            PaginaAtual = currentPage;
            TotalDePaginas = (int)Math.Ceiling(totalRecords / (double)pageSize);
        }
    }
}
