using System.Reflection;
using SPTarkov.Common.Models.Logging;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers.Server;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Spt.Tables;
using SPTarkov.Server.Core.Services.Locales;
using SPTarkov.Server.Core.Utils.Cloners;
using Path = System.IO.Path;

namespace StandardUnheardEdition;

[Injectable(TypePriority = OnLoadOrder.PostLoad + 1)]
public class Mod(
    ICloner cloner,
    ModHelper modHelper,
    LocaleService localeService,
    TemplateTable templateTable,
    ISptLogger<Mod> logger) : IOnLoad
{
    public Task OnLoadAsync(CancellationToken cancellationToken)
    {
        var pathToMod = modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
        var unheardProfileCopy = cloner.Clone(templateTable.Profiles["Unheard"])!;
        
        if (unheardProfileCopy.Bear?.Character?.Hideout is null 
            || unheardProfileCopy.Usec?.Character?.Hideout is null)
        {
            logger.Error("[Standard Unheard Edition] The profile cannot be copied");
            return Task.CompletedTask;
        }
        
        unheardProfileCopy.Bear.Character.Inventory = modHelper.GetJsonDataFromFile<BotBaseInventory>(
            pathToMod, Path.Combine("data", "bear_inventory.json"));
        unheardProfileCopy.Usec.Character.Inventory = modHelper.GetJsonDataFromFile<BotBaseInventory>(
            pathToMod, Path.Combine("data", "usec_inventory.json"));
        
        unheardProfileCopy.Bear.Character.Skills!.Common = modHelper.GetJsonDataFromFile<IEnumerable<CommonSkill>>(
            pathToMod, Path.Combine("data", "common_skill.json"));
        unheardProfileCopy.Usec.Character.Skills!.Common = modHelper.GetJsonDataFromFile<IEnumerable<CommonSkill>>(
            pathToMod, Path.Combine("data", "common_skill.json"));
        
        unheardProfileCopy.Bear.Character.Hideout.Areas = modHelper.GetJsonDataFromFile<List<BotHideoutArea>>(
            pathToMod, Path.Combine("data", "hideout_areas.json"));
        unheardProfileCopy.Usec.Character.Hideout.Areas = modHelper.GetJsonDataFromFile<List<BotHideoutArea>>(
            pathToMod, Path.Combine("data", "hideout_areas.json"));
        
        unheardProfileCopy.Bear.Character.Bonuses = modHelper.GetJsonDataFromFile<List<Bonus>>(
            pathToMod, Path.Combine("data", "bonuses.json"));
        unheardProfileCopy.Usec.Character.Bonuses = modHelper.GetJsonDataFromFile<List<Bonus>>(
            pathToMod, Path.Combine("data", "bonuses.json"));
        
        unheardProfileCopy.Bear.Trader = modHelper.GetJsonDataFromFile<ProfileTraderTemplate>(
            pathToMod, Path.Combine("data", "trader.json"));
        unheardProfileCopy.Usec.Trader = modHelper.GetJsonDataFromFile<ProfileTraderTemplate>(
            pathToMod, Path.Combine("data", "trader.json"));
        
        unheardProfileCopy.DescriptionLocaleKey = localeService.GetDesiredServerLocale() switch
        {
            "en" => "Standard profile with pockets and a secure container from Unheard.",
            "ru" => "Стандартный профиль с карманами и защищенным контейнером из Unheard.",
            _ => ""
        };
        
        templateTable.Profiles["Standard Unheard Edition"] = unheardProfileCopy;

        return Task.CompletedTask;
    }
}