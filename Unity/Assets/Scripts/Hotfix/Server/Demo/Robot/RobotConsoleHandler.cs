using System;
using System.Reflection;

namespace ET.Server
{
    [ConsoleHandler(ConsoleMode.Robot)]
    public class RobotConsoleHandler: IConsoleHandler
    {
        public async ETTask Run(Fiber fiber, ModeContex contex, string content)
        {
            string[] ss = content.Split(" ");
            switch (ss[0])
            {
                case ConsoleMode.Robot:
                    break;

                case "Run":
                {
                    int caseType = int.Parse(ss[1]);

                    try
                    {
                        fiber.Debug($"run case start: {caseType}");
                        await EventSystem.Instance.Invoke<RobotInvokeArgs, ETTask>(caseType, new RobotInvokeArgs() { Fiber = fiber, Content = content });
                        fiber.Debug($"run case finish: {caseType}");
                    }
                    catch (Exception e)
                    {
                        fiber.Debug($"run case error: {caseType}\n{e}");
                    }
                    break;
                }
                case "RunAll":
                {
                    FieldInfo[] fieldInfos = typeof (RobotCaseType).GetFields();
                    foreach (FieldInfo fieldInfo in fieldInfos)
                    {
                        int caseType = (int)fieldInfo.GetValue(null);
                        if (caseType > RobotCaseType.MaxCaseType)
                        {
                            fiber.Debug($"case > {RobotCaseType.MaxCaseType}: {caseType}");
                            break;
                        }
                        try
                        {
                            fiber.Debug($"run case start: {caseType}");
                            await EventSystem.Instance.Invoke<RobotInvokeArgs, ETTask>(caseType, new RobotInvokeArgs() { Fiber = fiber, Content = content});
                            fiber.Debug($"---------run case finish: {caseType}");
                        }
                        catch (Exception e)
                        {
                            fiber.Debug($"run case error: {caseType}\n{e}");
                            break;
                        }
                    }
                    break;
                }
            }
            await ETTask.CompletedTask;
        }
    }
}