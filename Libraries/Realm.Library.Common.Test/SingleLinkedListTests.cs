using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Realm.Library.Common.Test
{
    [TestFixture]
    public class SingleLinkedListTests
    {
        [Test]
        public void PushTest()
        {
            const int value = 1;

            var list = new SingleLinkedList<int>();
            list.Push(value);

            Assert.That(list.Head.Data, Is.EqualTo(value));
        }

        [Test]
        public void CountTest()
        {
            var list = new SingleLinkedList<int>();
            list.Push(1);
            list.Push(2);
            list.Push(3);

            Assert.That(list.Count, Is.EqualTo(3));
        }

        [Test]
        public void FirstTest()
        {
            var list = new SingleLinkedList<int>();
            list.Push(1);
            list.Push(2);

            var firstNode = list.First;

            Assert.That(firstNode, Is.Not.Null);
            Assert.That(firstNode.Data, Is.EqualTo(1));
        }

        [Test]
        public void NextTest()
        {
            var list = new SingleLinkedList<int>();
            list.Push(1);
            list.Push(2);

            var firstNode = list.First;

            Assert.That(firstNode.Next, Is.Not.Null);
            Assert.That(firstNode.Next.Data, Is.EqualTo(2));
        }

        [Test]
        public void LastTest()
        {
            var list = new SingleLinkedList<int>();
            list.Push(1);
            list.Push(2);

            var lastNode = list.Last;

            Assert.That(lastNode, Is.Not.Null);
            Assert.That(lastNode.Data, Is.EqualTo(2));
        }

        [Test]
        public void IsEmptyTest()
        {
            var list = new SingleLinkedList<int>();

            Assert.That(list.IsEmpty, Is.True);
        }

        [Test]
        public void PopTest()
        {
            var list = new SingleLinkedList<int>();
            list.Push(1);
            list.Push(2);

            var node = list.Pop();

            Assert.That(node, Is.Not.Null);
            Assert.That(node.Data, Is.EqualTo(1));
            Assert.That(list.Count, Is.EqualTo(1));
            Assert.That(list.First.Data, Is.EqualTo(2));
        }

        [Test]
        public void ReverseTest()
        {
            var list = new SingleLinkedList<int>();
            list.Push(1);
            list.Push(2);
            list.Push(3);

            list.Reverse();

            Assert.That(list.Count, Is.EqualTo(3));
            Assert.That(list.Head.Data, Is.EqualTo(3));
            Assert.That(list.Last.Data, Is.EqualTo(1));
        }

        [Test]
        public void ToEnumerableTest()
        {
            var list = new SingleLinkedList<int>();
            list.Push(1);
            list.Push(2);
            list.Push(3);

            var enumerableList = list.ToEnumerable();
            Assert.That(enumerableList, Is.Not.Null);

            var actualList = enumerableList.ToList();
            Assert.That(actualList, Is.Not.Null);
            Assert.That(actualList.Count, Is.EqualTo(3));
        }

        [Test]
        public void ReverseEnumeratorWithLinqTest()
        {
            var list = new SingleLinkedList<int>();
            list.Push(1);
            list.Push(2);
            list.Push(3);

            var reversedList = list.ToEnumerable().Reverse();

            var newList = new SingleLinkedList<int>();
            reversedList.ToList().ForEach(newList.Push);

            Assert.That(newList.Count, Is.EqualTo(3));
            Assert.That(newList.Head.Data, Is.EqualTo(3));
            Assert.That(newList.Last.Data, Is.EqualTo(1));
        }

        [Test]
        public void ReverseUsingHeadWithLinqTest()
        {
            var list = new SingleLinkedList<int>();
            list.Push(1);
            list.Push(2);
            list.Push(3);

            var newList = new List<SingleLinkedNode<int>>();

            var current = list.Head;
            while (current != null)
            {
                newList.Add(current);
                current = current.Next;
            }
            newList.Reverse();

            var reversedList = new SingleLinkedList<int>();
            newList.ForEach(node => reversedList.Push(node.Data));

            Assert.That(reversedList.Count, Is.EqualTo(3));
            Assert.That(reversedList.Head.Data, Is.EqualTo(3));
            Assert.That(reversedList.Last.Data, Is.EqualTo(1));
        }
    }
}
