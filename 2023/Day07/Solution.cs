using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using Microsoft.CodeAnalysis;
using System.Numerics;

namespace AdventOfCode.Y2023.Day07;

[ProblemName("Camel Cards")]
class Solution : Solver {

    public object PartOne(string input) {
        var rows = input.Split("\n");
        var hands = rows.Select(s => s.Split(" ")).Select(h => new Hand(h[0], h[1])).ToArray();

        return hands.OrderBy(x => x).Select((h, rank) => (rank + 1) * h.bid).Sum();
    }

    public object PartTwo(string input) {
        var rows = input.Split("\n");
        var hands = rows.Select(s => s.Split(" ")).Select(h => new Hand(h[0], h[1])).ToArray();

        return hands.OrderBy(x => x).Select((h, rank) => (rank + 1) * h.bid).Sum();
    }
}

public class Hand(string hand, string bid) : IComparable<Hand> {
    readonly Card[] cards = hand.Select(c => new Card(c)).ToArray();
    public readonly int bid = int.Parse(bid);

    public override string ToString() => $"{cards[0]}{cards[1]}{cards[2]}{cards[3]}{cards[4]} - {bid}";

    public int CompareTo(Hand other) {
        var compareValue = Part1(this).CompareTo(Part1(other));
        if (compareValue != 0) return compareValue;

        for (var i = 0; i < cards.Length; i++)
            if (cards[i].Worth - other.cards[i].Worth != 0) return (int)(cards[i].Worth - other.cards[i].Worth);

        return 0;
    }

    public static int Part1(Hand hand) => hand.cards.GroupBy(c => c.CardName).Count() switch {
        1 => 7,
        2 => FullHouseOrFourOfAKind(hand),
        3 => ThreeOfAKIndOrTwoPair(hand),
        4 => 2,
        _ => 1
    };

    private static int ThreeOfAKIndOrTwoPair(Hand hand) =>
        hand.cards.GroupBy(c => c.CardName).Where(x => x.Count() == 3).Any() ? 4 : 3;

    private static int FullHouseOrFourOfAKind(Hand hand) =>
        hand.cards.GroupBy(c => c.CardName).Where(x => x.Count() == 4).Any() ? 6 : 5;
}

public class Card(char cardName) : IComparable<Card> {
    public char CardName { get; set; } = cardName;
    public double Worth { get => CardWorth(CardName); }
    public override string ToString() => $"{CardName}";
    public int CompareTo(Card other) => Worth.CompareTo(other.Worth);

    public static double CardWorth(char CardName) {
        return CardName switch {
            'A' => 14,
            'K' => 13,
            'Q' => 12,
            'J' => 11,
            'T' => 10,
            _ => char.GetNumericValue(CardName)
        };
    }
}