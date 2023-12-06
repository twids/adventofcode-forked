using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2023.Day06;

[ProblemName("Wait For It")]
partial class Solution : Solver {

    public object PartOne(string input) {
        var rows = input.Split("\n");
        var times = MyRegex().Matches(rows[0]).Select(s => long.Parse(s.Value)).ToArray();
        var records = MyRegex().Matches(rows[1]).Select(s => long.Parse(s.Value)).ToArray();

        var count = 1;
        for (var i = 0; i < times.Length; i++)
            count *= Winning(times, records, i);

        return count;
    }

    public object PartTwo(string input) {
        var rows = input.Replace(" ", "").Split("\n");
        var time = MyRegex().Matches(rows[0]).Select(s => long.Parse(s.Value)).First();
        var record = MyRegex().Matches(rows[1]).Select(s => long.Parse(s.Value)).First();

        return Winning([time], [record], 0);
    }

    private static int Winning(long[] times, long[] records, int i) {
        var win = 0;
        for (var velocity = 0; velocity < times[i]; velocity++)
            if (velocity * (times[i] - velocity) > records[i]) win++;

        return win;
    }

    [GeneratedRegex(@"\d+")]
    private static partial Regex MyRegex();
}
