﻿using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using Hangfire;

using Listrr.Data;

namespace Listrr.Jobs.RecurringJobs
{

    [Queue("system")]
    public class GetLanguageCodesRecurringJob : IRecurringJob
    {

        private readonly AppDbContext _appDbContext;

        public GetLanguageCodesRecurringJob(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        public async Task Execute()
        {
            foreach (CultureInfo culture in CultureInfo.GetCultures(CultureTypes.NeutralCultures))
            {
                if (!_appDbContext.LanguageCodes.Any(x => x.Code == culture.TwoLetterISOLanguageName && x.Name == culture.NativeName))
                {
                    await _appDbContext.LanguageCodes.AddAsync(
                        new LanguageCode()
                        {
                            Code = culture.TwoLetterISOLanguageName,
                            Name = culture.NativeName
                        }
                    );

                    await _appDbContext.SaveChangesAsync();
                }
            }
        }
    }
}