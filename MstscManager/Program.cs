namespace MstscManager {
    internal static class Program {
        public static EventWaitHandle ProgramStarted;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            // 尝试创建一个命名事件
            bool createNew;
            ProgramStarted = new EventWaitHandle(false, EventResetMode.AutoReset, "MstscManager", out createNew);

            // 如果该命名事件已经存在(存在有前一个运行实例)，则发事件通知并退出
            if (!createNew) {
                ProgramStarted.Set();
                return;
            }

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new FMain());
        }
    }
}