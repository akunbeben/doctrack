using System;
using System.Linq;
using System.Linq.Expressions;

namespace DocTrack.Data.Validation
{
    public static class GenericValidation<Entity> where Entity : class
    {
        static object result;

        public static bool NameMustUnique(Entity entity, string fieldName, string userRequest, Expression<Func<Entity, bool>> expression)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                var data = _context.Set<Entity>();

                var parameter = Expression.Parameter(typeof(Entity), entity.GetType().Name);

                var parameterIsHidden = Expression.Property(parameter, "IsHidden");
                var parameterValueIsHidden = Expression.Constant(false, typeof(bool));
                var expressionBodyIsHidden = Expression.Equal(parameterIsHidden, parameterValueIsHidden);
                var filterIsHidden = Expression.Lambda<Func<Entity, bool>>(expressionBodyIsHidden, parameter);

                result = data.Where(filterIsHidden).SingleOrDefault(expression);

                var idOfEntity = typeof(Entity).GetProperties().First().GetValue(entity) as int?;

                if (result == null)
                {
                    return true;
                }

                var valueToCompare = result.GetType().GetProperty(fieldName).GetValue(entity).ToString();
                var idOfData = (int)typeof(Entity).GetProperties().First().GetValue(result);

                if (!idOfEntity.HasValue)
                {
                    return false;
                }

                if (valueToCompare == userRequest && idOfEntity == idOfData)
                {
                    return true;
                }

                return false;
            }
        }

        public static bool UniqueName(Entity entity, string fieldName, string userRequest, Expression<Func<Entity, bool>> expression)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                var data = _context.Set<Entity>();

                var parameter = Expression.Parameter(typeof(Entity), entity.GetType().Name);
                var parameterProperty = Expression.Property(parameter, fieldName);
                var parameterValue = Expression.Constant(userRequest, typeof(string));
                var expressionBody = Expression.Equal(parameterProperty, parameterValue);
                var filterValue = Expression.Lambda<Func<Entity, bool>>(expressionBody, parameter);


                result = data.SingleOrDefault(expression);

                var idOfEntity = typeof(Entity).GetProperties().First().GetValue(entity) as int?;

                if (result == null)
                {
                    return true;
                }

                var valueToCompare = result.GetType().GetProperty(fieldName).GetValue(entity).ToString();
                var idOfData = (int)typeof(Entity).GetProperties().First().GetValue(result);

                if (!idOfEntity.HasValue)
                {
                    return false;
                }

                if (valueToCompare == userRequest && idOfEntity == idOfData)
                {
                    return true;
                }

                return false;

                // End of this method.
            }
        }
    }
}
