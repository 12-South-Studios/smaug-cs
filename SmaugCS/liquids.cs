using System.Linq;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions;
using SmaugCS.Managers;

namespace SmaugCS
{
    public static class liquids
    {
        public static int MAX_COND_VALUE = 100;

        public static int figure_liq_vnum()
        {
            return db.LIQUIDS.Max(x => x.Vnum) + 1;
        }

        public static void displaymixture(CharacterInstance ch, MixtureData mix)
        {
            color.send_to_pager(" .-.                ,\r\n", ch);
            color.send_to_pager("`._ ,\r\n", ch);
            color.send_to_pager("   \\ \\                 o\r\n", ch);
            color.send_to_pager("    \\ `-,.\r\n", ch);
            color.send_to_pager("   .'o .  `.[]           o\r\n", ch);
            color.send_to_pager("<~- - , ,[].'.[] ~>     ___\r\n", ch);
            color.send_to_pager(" :               :     (-~.)\r\n", ch);
            color.send_to_pager("  `             '       `|'\r\n", ch);
            color.send_to_pager("   `           '         |\r\n", ch);
            color.send_to_pager("    `-.     .-'          |\r\n", ch);
            color.send_to_pager("-----{. _ _ .}-------------------\r\n", ch);

            color.pager_printf(ch, "&gRecipe for Mixture &G%s&g:\r\n", mix.Name);
            color.send_to_pager("---------------------------------\r\n", ch);

            if (!mix.Object)
            {
                LiquidData ingredient1 = db.GetLiquid(mix.Data[0]);
                LiquidData ingredient2 = db.GetLiquid(mix.Data[1]);
                color.send_to_pager("&wCombine two liquids to create this mixture:\r\n", ch);

                if (ingredient1 == null)
                    color.pager_printf(ch, "Vnum1 (%d) is invalid, tell an Admin\r\n", mix.Data[0]);
                else
                    color.pager_printf(ch, "&wOne part &G%s&w (%d)\r\n", ingredient1.Name, mix.Data[0]);

                if (ingredient2 == null)
                    color.pager_printf(ch, "Vnum2 (%d) is invalid, tell an Admin\r\n", mix.Data[1]);
                else
                    color.pager_printf(ch, "&wAnd part &G%s&w (%d)&D\r\n", ingredient2.Name, mix.Data[1]);
            }
            else
            {
                ObjectTemplate obj = DatabaseManager.Instance.OBJECT_INDEXES.Get(mix.Data[0]);
                if (obj == null)
                {
                    color.pager_printf(ch, "%s has a bad object vnum %d, inform an Admin\r\n", mix.Name, mix.Data[0]);
                    return;
                }

                LiquidData ingredient1 = db.GetLiquid(mix.Data[1]);
                color.send_to_pager("Combine an object and a liquid in this mixture\r\n", ch);
                color.pager_printf(ch, "&wMix &G%s&w (%d)\r\n", obj.Name, mix.Data[0]);
                color.pager_printf(ch, "&winto one part &G%s&w (%d)&D\r\n", ingredient1.Name, mix.Data[1]);
            }
        }

        public static LiquidData liq_can_mix(ObjectInstance sourceObj, ObjectInstance targetObj)
        {
            MixtureData mixture =
                db.MIXTURES.FirstOrDefault(m => m.Data[0] == sourceObj.Value[2]
                    || m.Data[1] == sourceObj.Value[2]);

            if (mixture == null || mixture.Data[2] == -1)
                return null;

            LiquidData liquid = db.GetLiquid(mixture.Data[2]);
            if (liquid == null)
                return null;

            sourceObj.Value[1] += targetObj.Value[1];
            sourceObj.Value[2] = liquid.Vnum;
            targetObj.Value[1] = 0;
            targetObj.Value[2] = -1;
            return liquid;
        }

        public static LiquidData liqobj_can_mix(ObjectInstance sourceObj, ObjectInstance objectLiq)
        {
            MixtureData mixture = db.MIXTURES.Where(m => (m.Data[0] == sourceObj.Value[2]
                                                                 || m.Data[1] == sourceObj.Value[2]))
                                     .FirstOrDefault(m => m.Data[0] == objectLiq.Value[2]
                                                                || m.Data[1] == objectLiq.Value[2]);

            if (mixture == null || mixture.Data[2] == -1)
                return null;

            LiquidData liquid = db.GetLiquid(mixture.Data[2]);
            if (liquid == null)
                return null;

            objectLiq.Value[1] += sourceObj.Value[1];
            objectLiq.Value[2] = liquid.Vnum;
            handler.separate_obj(sourceObj);
            sourceObj.FromCharacter();
            handler.extract_obj(sourceObj);
            return liquid;
        }
    }
}
