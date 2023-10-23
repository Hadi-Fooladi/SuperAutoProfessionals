using System.Reflection;
using SuperAutoProfessionals;

namespace ComposerApp;

internal class Program
{
    static void Main(string[] args)
    {
        var professionalType = typeof(Professional);

        var asm = professionalType.Assembly;
        var allTypes = asm.GetExportedTypes();

        Add(professionalType);

        foreach (var type in allTypes)
        {
            if (!type.IsClass) continue;
            if (type.IsSubclassOf(professionalType))
                Add(type);
        }

        Professional?[]
            left = BuildTeam("Left"),
            right = BuildTeam("Right");

        var leftTeam = new Team(left, Side.Left);
        var rightTeam = new Team(right, Side.Right);

        var game = new Game(leftTeam, rightTeam);
        var winner = game.RunTurn();
    }

    static Dictionary<string, Type> s_typeByCondeName = new();

    static void Add(Type type)
    {
        var obj = Activator.CreateInstance(type);

        var pro = (Professional)obj!;
        s_typeByCondeName[pro.CodeName] = type;
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
        if (s_typeByCondeName.TryGetValue(codeName, out var type))
            return (Professional)Activator.CreateInstance(type)!;

        return null;
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