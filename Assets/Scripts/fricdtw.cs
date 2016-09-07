using System;
using System.Linq;
using System.Collections.Generic;

namespace FricDTW
{
	public class tPoint
	{
		private double yDisp;
		private double time;
		
		public tPoint(double yDisp, double time)
		{
			this.yDisp = yDisp;
			this.time = time;
		}
		
		public static double dist(tPoint p1, tPoint p2)
		{
			double d = Math.Pow((p2.yDisp - p1.yDisp), 2);
			return Math.Sqrt(d);
		}

		public static double dist(double p1, double p2)
		{
			double d = Math.Pow((p2 - p1), 2);
			return Math.Sqrt(d);
		}

		public string ToString()
		{
			return string.Format("{0}, {1}", yDisp, time);
		}

		public static implicit operator double(tPoint t)
		{
			return t.yDisp;
		}

		public static implicit operator float(tPoint t)
		{
			return (float)t.yDisp;
		}

		public static implicit operator decimal(tPoint t)
		{
			return (decimal)t.yDisp;
		}
	}

	public class RecognizerDTW
	{
		private List<double> train;
		private double min;
		private double max;

        public const int DATA_X = 0;
        public const int DATA_Y = 1;
        public const int DATA_Z = 2;
        public const int DATA_T = 3;

        public RecognizerDTW() { }
		
		public RecognizerDTW(List<double> train)
		{
			this.train = train;
			min = train.Min();
			max = train.Max();
		}

        public RecognizerDTW(string data, int field)
        {
            List<double> train = new List<double>();
            string[] tdata = data.Split('\n');
            foreach(string line in tdata)
            {
				if(line == "") break;
                string[] d = line.Split(',');
                train.Add(Double.Parse(d[field]));
            }
            this.train = train;

			min = train.Min();
			max = train.Max();
        }


		//DTWDistance - gets the minimum distance mapping for two timeseries using DTW, returns minimum cumulative distance
		public double DTWDistance(List<double> input)
		{
			double[,] DTW = new double[train.Count, input.Count];
			
			for(int i = 1; i < train.Count; i++)
				DTW[i, 0] = Single.MaxValue;
			for(int i = 1; i < input.Count; i++)
				DTW[0, i] = Single.MaxValue;
			DTW[0, 0] = 0;
			
			for(int i = 1; i < train.Count; i++)
				for(int j = 1; j < input.Count; j++)
				{
					double trv = new double[] {DTW[i-1, j], DTW[i, j-1], DTW[i-1, j-1]}.Min();
					double cost = tPoint.dist(train[i], input[j]);
					DTW[i, j] = cost + trv;
				}
			
			return DTW[train.Count - 1, input.Count - 1];
		}

		//DTWDistanceWindow - same as DTWDistance, uses a locality window that doesn't allow mapping beyond a given threshold (int window)
		public double DTWDistanceWindow(List<double> input, int window)
		{
			if(Math.Abs(train.Count - input.Count) > window) window += (Math.Abs(train.Count - input.Count) - window);
			
			double[,] DTW = new double[train.Count, input.Count];
			
			for(int i = 0; i < train.Count; i++)
				for(int j = 0; j < input.Count; j++)
					DTW[i, j] = Single.MaxValue;
			DTW[0, 0] = 0;
			
			for(int i = 1; i < train.Count; i++)
				for(int j = Math.Max(1, i - window); j < Math.Min(input.Count, i + window); j++)
				{
					double trv = new double[] {DTW[i-1, j], DTW[i, j-1], DTW[i-1, j-1]}.Min();
					double cost = tPoint.dist(train[i], input[j]);
					DTW[i, j] = cost + trv;
				}
				
			return DTW[train.Count - 1, input.Count - 1];
		}

		public List<double> Training
		{
			get { return train; }
		}

		public double Min
		{
			get { return min; }
		}

		public double Max
		{
			get { return max; }
		}

		public double First
		{
			get { return train[0]; }
		}
	}

    public class SeriesRecognizer
    {
        private RecognizerDTW[][] activity;

        public const int ACT_RISE = 0;
        public const int ACT_CONT = 1;
        public const int ACT_FALL = 2;
        public const int MAXACT = 3;

        public SeriesRecognizer()
        {
            activity = new RecognizerDTW[MAXACT][];
            for (int i = 0; i < MAXACT; i++)
                for (int j = 0; j < RecognizerDTW.DATA_T; j++)
                    activity[i][j] = new RecognizerDTW();
        }

        public double Recognize(List<double> input, int field, int step)
        {
            return activity[step][field].DTWDistance(input);
        }
    }
}