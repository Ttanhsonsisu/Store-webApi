using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDBContext _context;
        public PortfolioRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            await _context.Portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();
            
            return portfolio;
        }

        public async Task<Portfolio?> DeletePortfolioAsync(AppUser appUser, string symbol)
        {
            var portfolioModel = await _context.Portfolios.FirstOrDefaultAsync(e => e.AppUserId == appUser.Id && e.Stock.Symbol.ToLower() == symbol);

            if(portfolioModel == null)
                return null;

            _context.Portfolios.Remove(portfolioModel);
            await _context.SaveChangesAsync();

            return portfolioModel;
        }

        public async Task<List<Stock>> GetUserPortfolio(AppUser appUser)
        {
            return await _context.Portfolios.Where(e => e.AppUserId == appUser.Id)
            .Select(stock => new Stock {
                ID = stock.Stock.ID,
                Symbol = stock.Stock.Symbol,
                CompanyName = stock.Stock.CompanyName,
                Purchase = stock.Stock.Purchase,
                LastDiv = stock.Stock.LastDiv,
                Industry = stock.Stock.Industry,
                MarketCap = stock.Stock.MarketCap
            }).ToListAsync();
        }
        
    }
}