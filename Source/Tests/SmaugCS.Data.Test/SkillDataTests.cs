using NUnit.Framework;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Data.Test
{
    [TestFixture]
    public class SkillDataTests
    {
        [Test]
        public void Compare_NullSkill1_Test()
        {
            var sk = new SkillData(1, "Test");
            
            Assert.That(SkillData.Compare(null, sk), Is.EqualTo(1));
        }

        [Test]
        public void Compare_NullSkill2_Test()
        {
            var sk = new SkillData(1, "Test");

            Assert.That(SkillData.Compare(sk, null), Is.EqualTo(-1));
        }

        [Test]
        public void Compare_BothNull_Test()
        {
            Assert.That(SkillData.Compare(null, null), Is.EqualTo(0));
        }

        [TestCase("Test", "Tester", 1)]
        [TestCase("Tester", "Test", -1)]
        [TestCase("Test", "Test", 0)]
        public void Compare_Test(string name1, string name2, int expectedValue)
        {
            var sk1 = new SkillData(1, name1);
            var sk2 = new SkillData(2, name2);

            Assert.That(SkillData.Compare(sk1, sk2), Is.EqualTo(expectedValue));
        }

        [Test]
        public void CompareByType_NullSkill1_Test()
        {
            var sk = new SkillData(1, "Test");

            Assert.That(SkillData.CompareByType(null, sk), Is.EqualTo(1));
        }

        [Test]
        public void CompareByType_NullSkill2_Test()
        {
            var sk = new SkillData(1, "Test");

            Assert.That(SkillData.CompareByType(sk, null), Is.EqualTo(-1));
        }

        [Test]
        public void CompareByType_BothNull_Test()
        {
            Assert.That(SkillData.CompareByType(null, null), Is.EqualTo(0));
        }

        [TestCase(SkillTypes.Spell, SkillTypes.Disease, -1)]
        [TestCase(SkillTypes.Tongue, SkillTypes.Skill, 1)]
        public void CompareByType_Test(SkillTypes type1, SkillTypes type2, int expectedValue)
        {
            var sk1 = new SkillData(1, "Test1");
            sk1.Type = type1;
            var sk2 = new SkillData(2, "Test2");
            sk2.Type = type2;

            Assert.That(SkillData.CompareByType(sk1, sk2), Is.EqualTo(expectedValue));
        }

        [TestCase("Test", "Tester", 1)]
        [TestCase("Tester", "Test", -1)]
        [TestCase("Test", "Test", 0)]
        public void CompareByType_CheckName_Test(string name1, string name2, int expectedValue)
        {
            var sk1 = new SkillData(1, name1);
            sk1.Type = SkillTypes.Herb;
            var sk2 = new SkillData(2, name2);
            sk2.Type = SkillTypes.Herb;

            Assert.That(SkillData.CompareByType(sk1, sk2), Is.EqualTo(expectedValue));
        }
    }
}
