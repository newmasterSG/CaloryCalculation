using CaloryCalculatiom.Domain.Entities;
using CaloryCalculatiom.Domain.Entities.Enums;
using CaloryCalculation.Application.DTOs.DailyLogs;
using CaloryCalculation.Application.DTOs.FoodConsumptions;
using CaloryCalculation.Application.DTOs.Nutrion;
using CaloryCalculation.Application.DTOs.Products;
using CaloryCalculation.Application.Interfaces;
using CaloryCalculation.Db;
using Microsoft.EntityFrameworkCore;

namespace CaloryCalculation.Application.Services
{
    public class DailyLogService(CaloryCalculationDbContext dbContext) : IDailyLogService
    {
        public async Task AddProductToDailyLogAsync(int userId, int productId, double quantity, DateTime date, MealType mealType, CancellationToken cancellationToken = default)
        {
            // Get the user's daily log for the specified date, or create a new one if it doesn't exist
            var dailyLog = await GetDailyLogForUserAsync(userId, date, cancellationToken);
            if (dailyLog == null)
            {
                dailyLog = new DailyLog
                {
                    UserId = userId,
                    Date = date
                };
                await dbContext.DailyLogs.AddAsync(dailyLog, cancellationToken);
            }

            var product = await dbContext.Products.FindAsync(new object[] { productId }, cancellationToken) ?? throw new Exception("Product not found.");
            var foodConsumption = new FoodConsumption
            {
                FoodItemId = productId,
                Product = product,
                Quantity = quantity,
                Date = date,
                MealType = mealType,
                DailyLog = dailyLog
            };

            dailyLog.FoodConsumptions.Add(foodConsumption);

            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<DailyLog?> GetDailyLogForUserAsync(int userId, DateTime date, CancellationToken cancellationToken = default)
        {
            return await dbContext.DailyLogs.Include(dl => dl.FoodConsumptions)
                                                .ThenInclude(fc => fc.Product)
                                              .Include(dl => dl.DailyLogExercises)
                                                .AsSplitQuery()
                                              .FirstOrDefaultAsync(dl => dl.UserId == userId && dl.Date.Date == date.Date, cancellationToken);
        }
        
        public async Task<DailyLogDTO?> GetDailyLogForUserAsync(GetDailyLogUserDTO getDailyLogUserDto, CancellationToken cancellationToken = default)
        {
            var dailyLog = await dbContext.DailyLogs.Include(dl => dl.FoodConsumptions)
                .ThenInclude(fc => fc.Product)
                .Include(dl => dl.DailyLogExercises)
                .AsSplitQuery()
                .FirstOrDefaultAsync(dl => dl.UserId == getDailyLogUserDto.UserId && dl.Date.Date == getDailyLogUserDto.Date, cancellationToken);

            if (dailyLog is null)
            {
                return await Task.FromResult(new DailyLogDTO());
            }

            var dto = new DailyLogDTO()
            {
                Id = dailyLog.Id,
                FoodConsumption = new List<FoodConsumptionDTO>(),
            };

            foreach (var fc in dailyLog.FoodConsumptions)
            {
                AddProductToMeal(dto, fc);
            }

            return dto;
        }

        public async Task<bool> DeleteProductFromDailyLogByUserIdDateAsync(int userId, int productId, MealType mealType,
            DateTime creationDate, CancellationToken cancellationToken = default)
        {
            var dailylog =
                await GetDailyLogForUserAsync(userId, creationDate, cancellationToken);
            
            ArgumentNullException.ThrowIfNull(dailylog);

            var foodcosumption =
                dailylog.FoodConsumptions.FirstOrDefault(fc => fc.MealType == mealType && fc.FoodItemId == productId);
            
            ArgumentNullException.ThrowIfNull(foodcosumption);

            dailylog.FoodConsumptions.Remove(foodcosumption);

            return await dbContext.SaveChangesAsync(cancellationToken) >= 1;
        }
        
        private void AddProductToMeal(DailyLogDTO dto, FoodConsumption fc)
        {
            var mealDictionary = dto.FoodConsumption.ToDictionary(x => x.MealType);

            if (!mealDictionary.TryGetValue((int)fc.MealType, out var meal))
            {
                meal = new FoodConsumptionDTO { MealType = (int)fc.MealType, Products = new List<ProductDTO>() };
                dto.FoodConsumption.Add(meal);
            }

            meal.Products.Add(CalculateTotalNutrition(fc));
        }
        
        private ProductDTO CalculateTotalNutrition(FoodConsumption fc)
        {

            var productDto = new ProductDTO
            {
                Id = fc.Product.Id,
                Name = fc.Product.Name,
                Calories = (fc.Product.Calories / fc.Product.PerGram) * fc.Quantity,
                Protein = (fc.Product.Protein / fc.Product.PerGram) * fc.Quantity,
                Fat = (fc.Product.Fat / fc.Product.PerGram) * fc.Quantity,
                Carb = (fc.Product.Сarbohydrate / fc.Product.PerGram) * fc.Quantity
            };

            return productDto;
        }
    }
}
