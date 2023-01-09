using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;
using TDL.Infrastructure.Extensions;
using TDL.Infrastructure.Persistence.Translations.DateTime;
using TDL.Infrastructure.Persistence.Translations.String;

namespace TDL.Infrastructure.Persistence.Translations
{
    public static class Translation
    {
        public static void Translate(this ModelBuilder modelBuilder, string requestedTimeZone)
        {
            TranslateEqualsInvariantFunc(modelBuilder);
            TranslateContainKeyWordInvariantFunc(modelBuilder);
            TranslateEqualToMonthWithOffsetFunc(modelBuilder, requestedTimeZone);
            TranslateEqualToYearWithOffsetFunc(modelBuilder, requestedTimeZone);
        }

        public static void TranslateEqualsInvariantFunc(ModelBuilder modelBuilder)
        {
            MethodInfo methodInfo = typeof(StringExtension).GetMethod(nameof(StringExtension.EqualsInvariant));

            if(methodInfo != null)
            {
                modelBuilder.HasDbFunction(methodInfo)
                    .HasTranslation(args => new EqualsInvariantExpression(args.First(), args.Last()));
            }
        }

        public static void TranslateContainKeyWordInvariantFunc(ModelBuilder modelBuilder)
        {
            MethodInfo methodInfo = typeof(StringExtension).GetMethod(nameof(StringExtension.ContainInvariant));

            if(methodInfo != null)
            {
                modelBuilder.HasDbFunction(methodInfo)
                    .HasTranslation(args => new ContainInvariantExpression(args.First(), args.Last()));
            }
        }

        private static void TranslateEqualToMonthWithOffsetFunc(ModelBuilder modelBuilder, string requestedTimeZone)
        {
            MethodInfo methodInfo = typeof(DateTimeExtension).GetMethod(nameof(DateTimeExtension.EqualToMonth));

            if(methodInfo != null)
            {
                modelBuilder.HasDbFunction(methodInfo)
                    .HasTranslation(args => new EqualToMonthWithOffsetExpression(args.First(), args.Last(), requestedTimeZone));
            }
        }

        private static void TranslateEqualToYearWithOffsetFunc(ModelBuilder modelBuilder, string requestedTimeZone)
        {
            MethodInfo methodInfo = typeof(DateTimeExtension).GetMethod(nameof(DateTimeExtension.EqualToYear));

            if(methodInfo != null)
            {
                modelBuilder.HasDbFunction(methodInfo)
                    .HasTranslation(args => new EqualToYearWithOffsetExpression(args.First(), args.Last(), requestedTimeZone));
            }
        }
    }
}
