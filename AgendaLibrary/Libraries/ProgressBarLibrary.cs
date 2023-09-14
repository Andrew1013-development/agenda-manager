namespace AgendaLibrary.Libraries
{
    public class ProgressBarLibrary
    {
        // characters
        private const char start_progress = '[';
        private const char end_progress = ']';
        private const char fill_progress = '#';
        private const char nonfill_progress = ' ';
        // progress bar
        private int width = Console.WindowWidth - 2 - 4;
        private double start;
        private double end;
        private int fill_count = 0;
        private int nonfill_count = Console.WindowWidth - 2 - 4;
        private double current_percent = 0.0;
        private bool bottom_alignment = false;
        // old cursor
        private int old_cursor_x = Console.CursorLeft;
        private int old_cursor_y = Console.CursorTop;

        public ProgressBarLibrary(double start, double end, bool bottom_alignment)
        {
            this.start = start;
            this.end = end;
            this.bottom_alignment = bottom_alignment;
        }

        public void UpdateProgress(double update)
        {
            // advance percentage
            this.start += update;
            this.current_percent = this.start / this.end;

            // calculate fill count
            this.fill_count = (int)Math.Round(current_percent * width,0);
            this.nonfill_count = width - fill_count;

            ShowProgress();
        }

        public void ShowProgress()
        {
            // position cursor
            if (bottom_alignment)
            {
                Console.SetCursorPosition(0, Console.WindowHeight);
            } else
            {
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.WriteLine(); // write a line to be safe
            }

            // draw progress bar every update
            Console.Write(start_progress);
            Console.Write(new string(fill_progress, fill_count)); // repeat string
            Console.Write(new string(nonfill_progress, nonfill_count)); // repeat string
            Console.Write(end_progress);
            Console.Write(Math.Min(100, Math.Round(current_percent * 100, 0))); // print to 100% only
            Console.Write("%");

            // return to old cursor position
            UpdateCursor(Console.CursorLeft, Console.CursorTop);
            Console.SetCursorPosition(old_cursor_x, old_cursor_y);

            // break out of progress
            if (start >= end)
            {        
                Console.WriteLine();
                DestroyProgress();
            }
        }
        
        internal void DestroyProgress()
        {
            this.current_percent = 0;
            this.fill_count = 0;
            this.nonfill_count = width - this.fill_count;
            Console.CursorLeft = old_cursor_x;
            Console.CursorTop = old_cursor_y;
        }

        public void BlockProgress()
        {
            Console.SetCursorPosition(old_cursor_x, old_cursor_y);
        }

        public void UpdateCursor(int x, int y)
        {
            this.old_cursor_x = x;
            this.old_cursor_y = y;
        }
    }
}
