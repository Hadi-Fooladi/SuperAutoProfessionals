using SuperAutoProfessionals;

namespace ComposerApp;

internal class Program
{
    static void Main(string[] args)
    {
        Professional?[]
            left = BuildTeam("Left"),
            right = BuildTeam("Right");

        var leftTeam = new Team(left, Side.Left);
        var rightTeam = new Team(right, Side.Right);

        var game = new Game(leftTeam, rightTeam);
        var winner = game.RunTurn();
    }

    public static Professional?[] BuildTeam(string side)
    {
        var result = new Professional?[5];
        for (int pos = 0; pos < 5; pos++)
        {
            var prompt = $"Enter {side} team professional at {pos + 1}";
            Console.WriteLine(prompt);
            Console.WriteLine(new string('-', prompt.Length));
            result[pos] = CreateProfessional();
        }
        return result;
    }

    public static Professional? CreateProfessional()
    {
        Professional? pro;

        for (;;)
        {
            Console.Write("Enter code name: ");
            var code = Console.ReadLine()!;
            if (code == "-") return null;

            pro = CreateProfessionalByCodeName(code);
            if (pro != null)
                break;
        }

        pro.Health = GetHealthOrAttack("Enter Health: ");
        pro.Attack = GetHealthOrAttack("Enter Attack: ");

        return pro;

    }

    public static Professional? CreateProfessionalByCodeName(string codeName)
    {
        switch (codeName)
        {
        default: return null;
        case "Bu": return new Buthcer();
        case "GD": return new GraveDigger();
        case "Nu": return new Nurse();
        case "Tr": return new Trainer();
        case "Pr": return new Professional();
        }
    }

    public static int GetHealthOrAttack(string prompt)
    {
        return GetNumber(prompt, 1, 50);
    }

    public static int GetNumber(string prompt, int min, int max)
    {
        for (; ; )
        {
            Console.Write(prompt);
            var line = Console.ReadLine();
            if (int.TryParse(line, out var num) && num >= min && num <= max)
                return num;
        }
    }
}