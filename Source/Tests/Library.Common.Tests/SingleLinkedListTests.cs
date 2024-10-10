using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Library.Common.LinkedList;
using Xunit;

namespace Library.Common.Tests;

public class SingleLinkedListTests
{
  [Fact]
  public void PushTest()
  {
    const int value = 1;

    SingleLinkedList<int> list = new();
    list.Push(value);

    list.Head.Data.Should().Be(value);
  }

  [Fact]
  public void CountTest()
  {
    SingleLinkedList<int> list = new();
    list.Push(1);
    list.Push(2);
    list.Push(3);

    list.Count.Should().Be(3);
  }

  [Fact]
  public void FirstTest()
  {
    SingleLinkedList<int> list = new();
    list.Push(1);
    list.Push(2);

    SingleLinkedNode<int> firstNode = list.First;

    firstNode.Should().NotBeNull();
    firstNode.Data.Should().Be(1);
  }

  [Fact]
  public void NextTest()
  {
    SingleLinkedList<int> list = new();
    list.Push(1);
    list.Push(2);

    SingleLinkedNode<int> firstNode = list.First;

    firstNode.Next.Should().NotBeNull();
    firstNode.Next.Data.Should().Be(2);
  }

  [Fact]
  public void LastTest()
  {
    SingleLinkedList<int> list = new();
    list.Push(1);
    list.Push(2);

    SingleLinkedNode<int> lastNode = list.Last;

    lastNode.Should().NotBeNull();
    lastNode.Data.Should().Be(2);
  }

  [Fact]
  public void IsEmptyTest()
  {
    SingleLinkedList<int> list = new();

    list.IsEmpty.Should().BeTrue();
  }

  [Fact]
  public void PopTest()
  {
    SingleLinkedList<int> list = new();
    list.Push(1);
    list.Push(2);

    SingleLinkedNode<int> node = list.Pop();

    node.Should().NotBeNull();
    node.Data.Should().Be(1);

    list.Count.Should().Be(1);
    list.First.Data.Should().Be(2);
  }

  [Fact]
  public void ReverseTest()
  {
    SingleLinkedList<int> list = new();
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
    SingleLinkedList<int> list = new();
    list.Push(1);
    list.Push(2);
    list.Push(3);

    IEnumerable<int> enumerableList = list.ToEnumerable();
    IEnumerable<int> enumerable = enumerableList.ToList();
    enumerable.Should().NotBeNull();

    List<int> actualList = enumerable.ToList();
    actualList.Should().NotBeNull();
    actualList.Count.Should().Be(3);
  }

  [Fact]
  public void ReverseEnumeratorWithLinqTest()
  {
    SingleLinkedList<int> list = new();
    list.Push(1);
    list.Push(2);
    list.Push(3);

    IEnumerable<int> reversedList = list.ToEnumerable().Reverse();

    SingleLinkedList<int> newList = new();
    reversedList.ToList().ForEach(newList.Push);

    newList.Count.Should().Be(3);
    newList.Head.Data.Should().Be(3);
    newList.Last.Data.Should().Be(1);
  }

  [Fact]
  public void ReverseUsingHeadWithLinqTest()
  {
    SingleLinkedList<int> list = new();
    list.Push(1);
    list.Push(2);
    list.Push(3);

    List<SingleLinkedNode<int>> newList = [];

    SingleLinkedNode<int> current = list.Head;
    while (current != null)
    {
      newList.Add(current);
      current = current.Next;
    }

    newList.Reverse();

    SingleLinkedList<int> reversedList = new();
    newList.ForEach(node => reversedList.Push(node.Data));

    reversedList.Count.Should().Be(3);
    reversedList.Head.Data.Should().Be(3);
    reversedList.Last.Data.Should().Be(1);
  }
}