using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stock;
using api.Models;

namespace api.Mappers
{
    public static class  StockMappers
    {
        public static StockDto ToStockDto(this Stock stockModel) 
        {
            return new StockDto{
                ID = stockModel.ID,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                LastDiv = stockModel.LastDiv,
                Purchase = stockModel.Purchase,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap,
                Comments = stockModel.comments.Select( c => c.ToCommentDto()).ToList()
            };
        }

        public static Stock ToStockFromCreateDTO (this CreateStockRequestDto stock) {
            return new Stock{
                Symbol = stock.Symbol,
                CompanyName = stock.CompanyName,
                LastDiv = stock.LastDiv,
                Purchase = stock.Purchase,
                Industry = stock.Industry,
                MarketCap = stock.MarketCap
            };
        }
    }
}