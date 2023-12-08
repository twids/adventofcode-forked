using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace AdventOfCode.Y2023.Day08;

[ProblemName("Haunted Wasteland")]
class Solution : Solver {

    public object PartOne(string input) {
        char[] order = input.Split("\n")[0].ToCharArray();
        var route = input.Split("\n").Skip(2).Select(s => Regex.Matches(s, @"\w{3}")).Select((r, rank) => new Route(rank, r[0].Value, r[1].Value, r[2].Value)).ToArray();
        var i = 0;
        var steps = 0;
        var currentStep = route.Where(r => r.name == "AAA").First();
        do {
            currentStep = NewMethod(order[i++], route, currentStep);
            if (i >= order.Length) i = 0; //starta om
            steps++;
        } while (currentStep.name != "ZZZ");
        return steps;
    }

    public object PartTwo(string input) {
        char[] order = input.Split("\n")[0].ToCharArray();
        var route = input.Split("\n").Skip(2).Select(s => Regex.Matches(s, @"\w{3}")).Select((r, rank) => new Route(rank, r[0].Value, r[1].Value, r[2].Value)).ToArray();

        var currentSteps = route.Where(r => r.name.EndsWith('A')).ToArray();

        long[] numbersToZ = new long[currentSteps.Length];
        Parallel.For(0, currentSteps.Length, j => {
            var steps = 0;
            var step = currentSteps[j];
            var i = 0;
            do {
                step = NewMethod(order[i++], route, step);
                if (i >= order.Length) i = 0; //starta om
                steps++;
            } while (!step.name.EndsWith('Z'));
            numbersToZ[j] = steps;
        });
        return LCMM(numbersToZ);
    }

    public static BigInteger LCMM(long[] inputs) => inputs.Length == 2 ? LCM(inputs[0], inputs[1]) : LCM(inputs[0], LCMM(inputs.Skip(1).ToArray()));

    public static BigInteger LCM(BigInteger a, BigInteger b) => a * b / GCD(a, b);

    public static BigInteger GCD(BigInteger a, BigInteger b) {
        while (b != 0) {
            var temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }
    private static Route NewMethod(char order, Route[] route, Route currentStep) => order switch {
        'L' => route.Where(r => r.name == currentStep.left).First(),
        'R' => route.Where(r => r.name == currentStep.right).First(),
        _ => throw new Exception("nup")
    };
}

internal class Route(int rank, string name, string left, string right) {
    public readonly int rank = rank;
    public readonly string name = name;
    public readonly string left = left;
    public readonly string right = right;
}