using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Realm.Library.Common.LinkedList;
using Xunit;

namespace Realm.Library.Common.Test
{
    public class SingleLinkedListTests
    {
        [Fact]
        public void PushTest()
        {
            const int value = 1;

            var list = new SingleLinkedList<int>();
            list.Push(value);

            list.Head.Data.Should().Be(value);
        }

        [Fact]
        public void CountTest()
        {
            var list = new SingleLinkedList<int>();
            list.Push(1);
            list.Push(2);
            list.Push(3);

            list.Count.Should().Be(3);
        }

        [Fact]
        public void FirstTest()
        {
            var list = new SingleLinkedList<int>();
            list.Push(1);
            list.Push(2);

            var firstNode = list.First;

            firstNode.Should().NotBeNull();
            firstNode.Data.Should().Be(1);
        }

        [Fact]
        public void NextTest()
        {
            var list = new SingleLinkedList<int>();
            list.Push(1);
            list.Push(2);

            var firstNode = list.First;

            firstNode.Next.Should().NotBeNull();
            firstNode.Next.Data.Should().Be(2);
        }

        [Fact]
        public void LastTest()
        {
            var list = new SingleLinkedList<int>();
            list.Push(1);
            list.Push(2);

            var lastNode = list.Last;

            lastNode.Should().NotBeNull();
            lastNode.Data.Should().Be(2);
        }

        [Fact]
        public void IsEmptyTest()
        {
            var list = new SingleLinkedList<int>();

            list.IsEmpty.Should().BeTrue();
        }

        [Fact]
        public void PopTest()
        {
            var list = new SingleLinkedList<int>();
            list.Push(1);
            list.Push(2);

            var node = list.Pop();

            node.Should().NotBeNull();
            node.Data.Should().Be(1);

            list.Count.Should().Be(1);
            list.First.Data.Should().Be(2);
        }

        [Fact]
        public void ReverseTest()
        {
            var list = new SingleLinkedList<int>();
            list.Push(1);
            list.Push(2);
            list.Push(3);

            list.Reverse();

            list.Count.Should().Be(3);
            list.Head.Data.Should().Be(3);
            list.Last.Data.Should().Be(1);
        }

        [Fact]
        public void ToEnumerableTest()
        {
            var list = new SingleLinkedList<int>();
            list.Push(1);
            list.Push(2);
            list.Push(3);

            var enumerableList = list.ToEnumerable();
            enumerableList.Should().NotBeNull();

            var actualList = enumerableList.ToList();
            actualList.Should().NotBeNull();
            actualList.Count.Should().Be(3);
        }

        [Fact]
        public void ReverseEnumeratorWithLinqTest()
        {
            var list = new SingleLinkedList<int>();
            list.Push(1);
            list.Push(2);
            list.Push(3);

            var reversedList = list.ToEnumerable().Reverse();

            var newList = new SingleLinkedList<int>();
            reversedList.ToList().ForEach(newList.Push);

            newList.Count.Should().Be(3);
            newList.Head.Data.Should().Be(3);
            newList.Last.Data.Should().Be(1);
        }

        [Fact]
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

            reversedList.Count.Should().Be(3);
            reversedList.Head.Data.Should().Be(3);
            reversedList.Last.Data.Should().Be(1);
        }
    }
}
