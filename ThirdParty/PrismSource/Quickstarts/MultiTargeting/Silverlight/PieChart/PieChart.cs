//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================
//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PieChartLibrary
{
    [ContentProperty("Sections")]
    public class PieChart : Canvas
    {
        private static readonly Brush PieStroke = new SolidColorBrush(Colors.Black);
        private static readonly Color[] SectionColors;
        private const double LabelsGap = 5d;
        
        private readonly double LabelsRectangleRadius;
        private double TotalWeight;

        public static readonly DependencyProperty CenterProperty =
           DependencyProperty.Register("Center", typeof(Point), typeof(PieChart), new PropertyMetadata(OnDependencyPropertyChanged));

        public static readonly DependencyProperty RadiusProperty =
           DependencyProperty.Register("Radius", typeof(double), typeof(PieChart), new PropertyMetadata(OnDependencyPropertyChanged));

        public static readonly DependencyProperty SectionsProperty =
           DependencyProperty.Register("Sections", typeof(ObservableCollection<PieSection>), typeof(PieChart), new PropertyMetadata(OnDependencyPropertyChanged));

        public static readonly DependencyProperty StrokeThicknessProperty =
           DependencyProperty.Register("StrokeThickness", typeof(double), typeof(PieChart), new PropertyMetadata(OnDependencyPropertyChanged));

        public static readonly DependencyProperty LabelsFontSizeProperty =
           DependencyProperty.Register("LabelsFontSize", typeof(double), typeof(PieChart), new PropertyMetadata(OnDependencyPropertyChanged));


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static PieChart()
        {
            SectionColors = new Color[10];
            SectionColors[0] = Color.FromArgb(0xFF,0,0,0xFF);           // blue
            SectionColors[1] = Color.FromArgb(0xFF, 0xB2, 0x22, 0x22);  // firebrick
            SectionColors[2] = Color.FromArgb(0xFF, 0xFF, 0, 0xFF);     // magenta
            SectionColors[3] = Color.FromArgb(0xFF, 0, 0x80, 0);        // green
            SectionColors[4] = Color.FromArgb(0xFF, 0x4B, 0, 0x80);     // indigo
            SectionColors[5] = Color.FromArgb(0xFF, 0xFF, 0, 0);        // red
            SectionColors[6] = Color.FromArgb(0xFF, 0xFF, 0xA5, 0);     // orange
            SectionColors[7] = Color.FromArgb(0xFF, 0, 0, 0);           // black
            SectionColors[8] = Color.FromArgb(0xFF, 0xA0, 0x52, 0x2D);  // sienna
            SectionColors[9] = Color.FromArgb(0xFF, 0x80, 0x80, 0x80);  // grey
        }

        public PieChart()
        {
            StrokeThickness = 1d;
            LabelsFontSize = 10d;

            LabelsRectangleRadius = LabelsFontSize;
        }

        public Point Center
        {
            get { return (Point)GetValue(CenterProperty); }
            set { SetValue(CenterProperty, value); }
        }

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ObservableCollection<PieSection> Sections
        {
            get { return (ObservableCollection<PieSection>)GetValue(SectionsProperty); }
            set { SetValue(SectionsProperty, value); }
        }

        public double StrokeThickness
        {
            get { return (double) GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public double LabelsFontSize
        {
            get { return (double) GetValue(LabelsFontSizeProperty); }
            set { SetValue(LabelsFontSizeProperty, value); }
        }

        private double TopLabelsSection
        {
            get
            {
                return this.Center.Y + this.Radius + LabelsGap;
            }
        }

        private double LeftLabelsSection
        {
            get
            {
                return this.Center.X - this.Radius;
            }
        }

        private double LabelsSectionHeight
        {
            get
            {
                if (Sections != null)
                    return Sections.Count * (LabelsFontSize + LabelsGap);

                return 0d;
            }
        }

        private void SectionsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            LayoutPieChart();
        }

        private static void OnDependencyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PieChart chart = ((PieChart) d);
            chart.LayoutPieChart();
            ObservableCollection<PieSection> newSections = e.NewValue as ObservableCollection<PieSection>;
            if (newSections != null)
            {
                newSections.CollectionChanged += chart.SectionsChanged;
            }

            ObservableCollection<PieSection> oldSections = e.OldValue as ObservableCollection<PieSection>;
            if (oldSections != null)
            {
                oldSections.CollectionChanged -= chart.SectionsChanged;
            }
        }

        private void LayoutPieChart()
        {
            this.Children.Clear();

            if (Sections != null)
            {
                TotalWeight = Sections.Sum(s => s.SectionWeight);
                double startAngle = 0d;

                for (int index = 0; index < Sections.Count; index++ )
                {
                    startAngle = LayoutSection(Sections[index], index, startAngle);
                }
            }
        }

        private double LayoutSection(PieSection section, int sectionNumber, double sectionStartAngle)
        {
            // calculate section area based on its weight
            double sectionsAngle = (360d * section.SectionWeight)  / TotalWeight;

            double sectionEndAngle = sectionStartAngle + sectionsAngle;

            // draw section
            Color sectionColor = SectionColors[sectionNumber % SectionColors.Length];
            sectionColor.A = 0x20;
            DrawPieSection(sectionStartAngle, sectionEndAngle, sectionColor, true);

            // draw matching
            double matchingEndAngle = sectionStartAngle + ((sectionsAngle * section.MatchingPercentage) / 100d);
            sectionColor.A = 0xFF;
            DrawPieSection(sectionStartAngle, matchingEndAngle, sectionColor, false);

            // labels
            double topProperty = TopLabelsSection + ((LabelsFontSize + LabelsGap) * sectionNumber);
            RectangleGeometry labelRect = new RectangleGeometry
                                              {
                                                  Rect = new Rect(0d, 0d, LabelsRectangleRadius, LabelsRectangleRadius)
                                              };
            Path pathLabelRect = new Path
                                     {
                                         Fill = new SolidColorBrush(sectionColor),
                                         Data = labelRect,
                                         Stroke = PieStroke,
                                         StrokeThickness = StrokeThickness,
                                     };
            Canvas.SetTop(pathLabelRect, topProperty);
            Canvas.SetLeft(pathLabelRect, LeftLabelsSection);
            this.Children.Add(pathLabelRect);


            double leftProperty = LeftLabelsSection + (LabelsRectangleRadius + LabelsGap);
            TextBlock label = new TextBlock
                                  {
                                      Text = string.Format(CultureInfo.CurrentUICulture, "{0} ({1} %)", section.Description, section.MatchingPercentage),
                                      FontSize = LabelsFontSize
                                  };

            Canvas.SetTop(label, topProperty);
            Canvas.SetLeft(label, leftProperty);

            this.Children.Add(label);

            return sectionEndAngle;
        }

        private void DrawPieSection(double startAngle, double endAngle, Color color, bool showLines)
        {
            // Calculate start and end circumference points for the section
            Point A = CalculateCircumferencePoint(startAngle, Radius);
            Point B = CalculateCircumferencePoint(endAngle, Radius);

            PathSegmentCollection segments = new PathSegmentCollection();
            segments.Add(new LineSegment { Point = this.Center });

            segments.Add(new LineSegment { Point = B });
            segments.Add(new ArcSegment
                             {
                                 Size = new Size(Radius, Radius),
                                 Point = A,
                                 SweepDirection = SweepDirection.Counterclockwise,
                             });

            Path segmentPath = new Path
            {
                StrokeLineJoin = PenLineJoin.Round,
                StrokeThickness = StrokeThickness,
                Fill = new SolidColorBrush(color),
                Data = new PathGeometry
                {
                    Figures = new PathFigureCollection
                                  {
                                    new PathFigure
                                        {
                                            IsClosed = true,
                                            StartPoint = A,
                                            Segments = segments
                                        }
                                  }
                }
            };

            if (showLines)
            {
                segmentPath.Stroke = PieStroke;
            }

            this.Children.Add(segmentPath);
        }

        private Point CalculateCircumferencePoint(double angle, double radius)
        {
            double angleRad = (Math.PI / 180.0) * angle;

            double x = this.Center.X + radius * Math.Cos(angleRad);
            double y = this.Center.Y + radius * Math.Sin(angleRad);

            return new Point(x, y);
        }

    }
}
