using System.Linq;
using Realm.Library.Common;
using Realm.Library.Patterns.Repository;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions.Character;
using SmaugCS.Extensions.Objects;
using SmaugCS.Repository;

namespace SmaugCS
{
    public static class liquids
    {
        public static int MAX_COND_VALUE = 100;

        public static void displaymixture(CharacterInstance ch, MixtureData mix)
        {
            ch.SendToPager(" .-.                ,\r\n");
            ch.SendToPager("`._ ,\r\n");
            ch.SendToPager("   \\ \\                 o\r\n");
            ch.SendToPager("    \\ `-,.\r\n");
            ch.SendToPager("   .'o .  `.[]           o\r\n");
            ch.SendToPager("<~- - , ,[].'.[] ~>     ___\r\n");
            ch.SendToPager(" :               :     (-~.)\r\n");
            ch.SendToPager("  `             '       `|'\r\n");
            ch.SendToPager("   `           '         |\r\n");
            ch.SendToPager("    `-.     .-'          |\r\n");
            ch.SendToPager("-----{. _ _ .}-------------------\r\n");

            ch.PagerPrintf("&gRecipe for Mixture &G%s&g:\r\n", mix.Name);
            ch.SendToPager("---------------------------------\r\n");

            if (!mix.Object)
            {
                var ingredient1 = RepositoryManager.Instance.GetEntity<LiquidData>(mix.Data.ToList()[0]);
                var ingredient2 = RepositoryManager.Instance.GetEntity<LiquidData>(mix.Data.ToList()[1]);
                ch.SendToPager("&wCombine two liquids to create this mixture:");

                if (ingredient1 == null)
                    ch.PagerPrintf("Vnum1 (%d) is invalid, tell an Admin\r\n", mix.Data.ToList()[0]);
                else
                    ch.PagerPrintf("&wOne part &G%s&w (%d)\r\n", ingredient1.Name, mix.Data.ToList()[0]);

                if (ingredient2 == null)
                    ch.PagerPrintf("Vnum2 (%d) is invalid, tell an Admin\r\n", mix.Data.ToList()[1]);
                else
                    ch.PagerPrintf("&wAnd part &G%s&w (%d)&D\r\n", ingredient2.Name, mix.Data.ToList()[1]);
            }
            else
            {
                var obj = RepositoryManager.Instance.OBJECTTEMPLATES.CastAs<Repository<long, ObjectTemplate>>().Get(mix.Data.ToList()[0]);
                if (obj == null)
                {
                    ch.PagerPrintf("%s has a bad object vnum %d, inform an Admin\r\n", mix.Name, mix.Data.ToList()[0]);
                    return;
                }

                var ingredient1 = RepositoryManager.Instance.GetEntity<LiquidData>(mix.Data.ToList()[1]);
                ch.SendToPager("Combine an object and a liquid in this mixture");
                ch.PagerPrintf("&wMix &G%s&w (%d)\r\n", obj.Name, mix.Data.ToList()[0]);
                ch.PagerPrintf("&winto one part &G%s&w (%d)&D\r\n", ingredient1.Name, mix.Data.ToList()[1]);
            }
        }

        public static LiquidData liq_can_mix(ObjectInstance sourceObj, ObjectInstance targetObj)
        {
            var mixture =
                RepositoryManager.Instance.MIXTURES.Values.FirstOrDefault(m => m.Data.ToList()[0] == sourceObj.Value.ToList()[2]
                    || m.Data.ToList()[1] == sourceObj.Value.ToList()[2]);

            if (mixture == null || mixture.Data.ToList()[2] == -1)
                return null;

            var liquid = RepositoryManager.Instance.GetEntity<LiquidData>(mixture.Data.ToList()[2]);
            if (liquid == null)
                return null;

            sourceObj.Value.ToList()[1] += targetObj.Value.ToList()[1];
            sourceObj.Value.ToList()[2] = liquid.Vnum;
            targetObj.Value.ToList()[1] = 0;
            targetObj.Value.ToList()[2] = -1;
            return liquid;
        }

        public static LiquidData liqobj_can_mix(ObjectInstance sourceObj, ObjectInstance objectLiq)
        {
            var mixture = RepositoryManager.Instance.MIXTURES.Values.Where(m => m.Data.ToList()[0] == sourceObj.Value.ToList()[2]
                                                                                || m.Data.ToList()[1] == sourceObj.Value.ToList()[2])
                                     .FirstOrDefault(m => m.Data.ToList()[0] == objectLiq.Value.ToList()[2]
                                                                || m.Data.ToList()[1] == objectLiq.Value.ToList()[2]);

            if (mixture == null || mixture.Data.ToList()[2] == -1)
                return null;

            var liquid = RepositoryManager.Instance.GetEntity<LiquidData>(mixture.Data.ToList()[2]);
            if (liquid == null)
                return null;

            objectLiq.Value.ToList()[1] += sourceObj.Value.ToList()[1];
            objectLiq.Value.ToList()[2] = liquid.Vnum;
            //handler.separate_obj(sourceObj);
            sourceObj.RemoveFrom();
            sourceObj.Extract();
            return liquid;
        }
    }
}
