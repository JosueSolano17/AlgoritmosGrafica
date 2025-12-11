using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinAPPLineas.Clases;

namespace WinAPPLineas.Formularios
{
    public partial class ScanlineFillForm : Form
    {
        private PictureBox picCanvas;
        private Panel pnlControls;
        private GroupBox grpColors;
        private GroupBox grpAnimation;
        private GroupBox grpVisualization;
        private GroupBox grpGrid;
        private GroupBox grpRectangle;
        private Button btnFillColor;
        private Button btnBackgroundColor;
        private Button btnDrawRectangle;
        private Button btnStartFill;
        private Button btnClear;
        private Button btnStop;
        private CheckBox chkAnimate;
        private CheckBox chkShowExploration;
        private CheckBox chkShowBacktrack;
        private CheckBox chkShowGrid;
        private CheckBox chkShowNumbers;
        private CheckBox chkShowScanlines;
        private TrackBar trkSpeed;
        private NumericUpDown numCellSize;
        private NumericUpDown numRectWidth;
        private NumericUpDown numRectHeight;
        private Label lblSpeed;
        private Label lblSpeedValue;
        private Label lblCellSize;
        private Label lblRectWidth;
        private Label lblRectHeight;
        private Label lblInstructions;
        private Label lblStats;
        private ColorDialog colorDialog;

        private Bitmap canvas;
        private Bitmap displayBitmap;
        private Graphics gfx;
        private Graphics displayGfx;
        private Color fillColor = Color.FromArgb(0, 150, 136); // Verde azulado (Teal)
        private Color backgroundColor = Color.White;
        private Color boundaryColor = Color.FromArgb(233, 30, 99); // Rosa intenso
        private Color scanlineColor = Color.FromArgb(150, 255, 235, 59); // Amarillo
        private Color explorationColor = Color.FromArgb(150, 3, 169, 244); // Azul claro
        private Color gridColor = Color.FromArgb(189, 189, 189); // Gris claro
        private Color bgColor = Color.FromArgb(250, 250, 250); // Fondo claro
        private bool isAnimating = false;
        private CancellationTokenSource cancellationTokenSource;
        private int pixelsFilled = 0;
        private int neighborsExplored = 0;
        private int scanlinesProcessed = 0;

        // Sistema de cuadrícula
        private int cellSize = 20;
        private int gridWidth = 40;
        private int gridHeight = 30;
        private int[,] grid; // 0 = vacío, 1 = límite, 2 = rellenado

        public ScanlineFillForm()
        {
            InitializeComponent();
            InitializeCustomComponents();
            SetupCanvas();
        }

        private void InitializeCustomComponents()
        {
            // Configuración del formulario
            this.Text = "Scanline Fill - Rectángulo Animado";
            this.Size = new Size(1150, 850);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 245, 245);

            // Panel de controles lateral
            pnlControls = new Panel
            {
                Dock = DockStyle.Right,
                Width = 280,
                BackColor = Color.FromArgb(33, 33, 33),
                Padding = new Padding(10),
                AutoScroll = true
            };

            // GroupBox de cuadrícula
            grpGrid = new GroupBox
            {
                Text = "Configuración de Cuadrícula",
                Location = new Point(10, 10),
                Size = new Size(260, 80),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(224, 224, 224)
            };

            lblCellSize = new Label
            {
                Text = "Tamaño de celda:",
                Location = new Point(15, 25),
                Size = new Size(120, 20),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(224, 224, 224)
            };

            numCellSize = new NumericUpDown
            {
                Location = new Point(140, 23),
                Size = new Size(100, 25),
                Minimum = 10,
                Maximum = 50,
                Value = 20,
                Font = new Font("Segoe UI", 9F)
            };
            numCellSize.ValueChanged += NumCellSize_ValueChanged;

            chkShowGrid = new CheckBox
            {
                Text = "Mostrar líneas de cuadrícula",
                Location = new Point(15, 50),
                Size = new Size(230, 25),
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(224, 224, 224),
                Checked = true
            };
            chkShowGrid.CheckedChanged += (s, e) => RedrawDisplay();

            grpGrid.Controls.AddRange(new Control[] { lblCellSize, numCellSize, chkShowGrid });

            // GroupBox de rectángulo
            grpRectangle = new GroupBox
            {
                Text = "Dimensiones del Rectángulo",
                Location = new Point(10, 100),
                Size = new Size(260, 110),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(224, 224, 224)
            };

            lblRectWidth = new Label
            {
                Text = "Ancho (celdas):",
                Location = new Point(15, 25),
                Size = new Size(120, 20),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(224, 224, 224)
            };

            numRectWidth = new NumericUpDown
            {
                Location = new Point(140, 23),
                Size = new Size(100, 25),
                Minimum = 5,
                Maximum = 30,
                Value = 15,
                Font = new Font("Segoe UI", 9F)
            };

            lblRectHeight = new Label
            {
                Text = "Alto (celdas):",
                Location = new Point(15, 55),
                Size = new Size(120, 20),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(224, 224, 224)
            };

            numRectHeight = new NumericUpDown
            {
                Location = new Point(140, 53),
                Size = new Size(100, 25),
                Minimum = 5,
                Maximum = 20,
                Value = 10,
                Font = new Font("Segoe UI", 9F)
            };

            grpRectangle.Controls.AddRange(new Control[] { lblRectWidth, numRectWidth, lblRectHeight, numRectHeight });

            // GroupBox de colores
            grpColors = new GroupBox
            {
                Text = "Colores",
                Location = new Point(10, 220),
                Size = new Size(260, 120),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(224, 224, 224)
            };

            btnFillColor = new Button
            {
                Text = "Color de Relleno",
                Location = new Point(15, 25),
                Size = new Size(230, 35),
                BackColor = fillColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F),
                Cursor = Cursors.Hand
            };
            btnFillColor.FlatAppearance.BorderSize = 1;
            btnFillColor.Click += BtnFillColor_Click;

            btnBackgroundColor = new Button
            {
                Text = "Color de Límite",
                Location = new Point(15, 70),
                Size = new Size(230, 35),
                BackColor = boundaryColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F),
                Cursor = Cursors.Hand
            };
            btnBackgroundColor.FlatAppearance.BorderSize = 1;
            btnBackgroundColor.Click += BtnBackgroundColor_Click;

            grpColors.Controls.AddRange(new Control[] { btnFillColor, btnBackgroundColor });

            // GroupBox de animación
            grpAnimation = new GroupBox
            {
                Text = "Velocidad de Animación",
                Location = new Point(10, 350),
                Size = new Size(260, 140),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(224, 224, 224)
            };

            chkAnimate = new CheckBox
            {
                Text = "Activar animación paso a paso",
                Location = new Point(15, 25),
                Size = new Size(230, 25),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(224, 224, 224),
                Checked = true
            };
            chkAnimate.CheckedChanged += ChkAnimate_CheckedChanged;

            lblSpeed = new Label
            {
                Text = "Velocidad:",
                Location = new Point(15, 60),
                Size = new Size(80, 20),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(224, 224, 224)
            };

            lblSpeedValue = new Label
            {
                Text = "Media",
                Location = new Point(180, 60),
                Size = new Size(65, 20),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 150, 136),
                TextAlign = ContentAlignment.MiddleRight
            };

            trkSpeed = new TrackBar
            {
                Location = new Point(15, 85),
                Size = new Size(230, 45),
                Minimum = 0,
                Maximum = 100,
                Value = 50,
                TickFrequency = 10,
                Enabled = true
            };
            trkSpeed.ValueChanged += TrkSpeed_ValueChanged;

            grpAnimation.Controls.AddRange(new Control[] { chkAnimate, lblSpeed, lblSpeedValue, trkSpeed });

            // GroupBox de visualización
            grpVisualization = new GroupBox
            {
                Text = "Opciones de Visualización",
                Location = new Point(10, 500),
                Size = new Size(260, 150),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(224, 224, 224)
            };

            chkShowScanlines = new CheckBox
            {
                Text = "Mostrar líneas de escaneo",
                Location = new Point(15, 25),
                Size = new Size(230, 25),
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(224, 224, 224),
                Checked = true
            };

            chkShowExploration = new CheckBox
            {
                Text = "Mostrar exploración horizontal",
                Location = new Point(15, 55),
                Size = new Size(230, 25),
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(224, 224, 224),
                Checked = true
            };

            chkShowBacktrack = new CheckBox
            {
                Text = "Mostrar exploración vertical",
                Location = new Point(15, 85),
                Size = new Size(230, 25),
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(224, 224, 224),
                Checked = false
            };

            chkShowNumbers = new CheckBox
            {
                Text = "Mostrar números de orden",
                Location = new Point(15, 115),
                Size = new Size(230, 25),
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(224, 224, 224),
                Checked = true
            };

            grpVisualization.Controls.AddRange(new Control[] { chkShowScanlines, chkShowExploration, chkShowBacktrack, chkShowNumbers });

            // Label de estadísticas
            lblStats = new Label
            {
                Text = "Píxeles rellenados: 0\nLíneas procesadas: 0\nVecinos explorados: 0",
                Location = new Point(10, 660),
                Size = new Size(260, 60),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(224, 224, 224),
                BackColor = Color.FromArgb(48, 48, 48),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(5),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Botones de acción
            btnDrawRectangle = new Button
            {
                Text = "📐 Dibujar Rectángulo",
                Location = new Point(10, 730),
                Size = new Size(260, 40),
                BackColor = Color.FromArgb(233, 30, 99),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnDrawRectangle.FlatAppearance.BorderSize = 0;
            btnDrawRectangle.Click += BtnDrawRectangle_Click;

            btnStartFill = new Button
            {
                Text = "▶ Iniciar Relleno",
                Location = new Point(10, 780),
                Size = new Size(260, 40),
                BackColor = Color.FromArgb(0, 150, 136),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnStartFill.FlatAppearance.BorderSize = 0;
            btnStartFill.Click += BtnStartFill_Click;

            btnClear = new Button
            {
                Text = "🗑 Limpiar",
                Location = new Point(10, 830),
                Size = new Size(125, 40),
                BackColor = Color.FromArgb(120, 120, 120),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnClear.FlatAppearance.BorderSize = 0;
            btnClear.Click += BtnClear_Click;

            btnStop = new Button
            {
                Text = "⏹ Detener",
                Location = new Point(145, 830),
                Size = new Size(125, 40),
                BackColor = Color.FromArgb(244, 67, 54),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnStop.FlatAppearance.BorderSize = 0;
            btnStop.Click += BtnStop_Click;

            // Label de instrucciones
            lblInstructions = new Label
            {
                Text = "📋 INSTRUCCIONES:\n\n" +
                       "1. Configura las dimensiones\n" +
                       "2. Dibuja el rectángulo\n" +
                       "3. Inicia el relleno animado\n\n" +
                       "El algoritmo rellena línea\n" +
                       "por línea horizontalmente.",
                Location = new Point(10, 880),
                Size = new Size(260, 140),
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(224, 224, 224),
                BackColor = Color.FromArgb(48, 48, 48),
                Padding = new Padding(8),
                BorderStyle = BorderStyle.FixedSingle
            };

            pnlControls.Controls.AddRange(new Control[] {
                grpGrid, grpRectangle, grpColors, grpAnimation, grpVisualization,
                lblStats, btnDrawRectangle, btnStartFill, btnClear, btnStop, lblInstructions
            });

            // PictureBox para el lienzo
            picCanvas = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = bgColor,
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.CenterImage,
                Cursor = Cursors.Cross
            };

            colorDialog = new ColorDialog
            {
                FullOpen = true
            };

            this.Controls.Add(picCanvas);
            this.Controls.Add(pnlControls);
        }

        private void SetupCanvas()
        {
            cellSize = (int)numCellSize.Value;
            canvas = new Bitmap(gridWidth, gridHeight);
            displayBitmap = new Bitmap(gridWidth * cellSize, gridHeight * cellSize);
            grid = new int[gridHeight, gridWidth];

            gfx = Graphics.FromImage(canvas);
            displayGfx = Graphics.FromImage(displayBitmap);

            ClearCanvas();
        }

        private void NumCellSize_ValueChanged(object sender, EventArgs e)
        {
            if (!isAnimating)
            {
                SetupCanvas();
            }
        }

        private void ClearCanvas()
        {
            // Limpiar el canvas lógico y la cuadrícula
            gfx.Clear(backgroundColor);
            for (int y = 0; y < gridHeight; y++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    grid[y, x] = 0;
                    canvas.SetPixel(x, y, backgroundColor);
                }
            }

            ResetStats();
            RedrawDisplay();
        }

        private void BtnDrawRectangle_Click(object sender, EventArgs e)
        {
            ClearCanvas();

            int rectWidth = (int)numRectWidth.Value;
            int rectHeight = (int)numRectHeight.Value;

            // Asegurar que el rectángulo quepa en la cuadrícula
            rectWidth = Math.Min(rectWidth, gridWidth - 2);
            rectHeight = Math.Min(rectHeight, gridHeight - 2);

            // Calcular posición central
            int startX = (gridWidth - rectWidth) / 2;
            int startY = (gridHeight - rectHeight) / 2;

            // Dibujar el rectángulo en la cuadrícula y el canvas
            for (int x = startX; x < startX + rectWidth; x++)
            {
                grid[startY, x] = 1; // Borde superior
                grid[startY + rectHeight - 1, x] = 1; // Borde inferior
                canvas.SetPixel(x, startY, boundaryColor);
                canvas.SetPixel(x, startY + rectHeight - 1, boundaryColor);
            }

            for (int y = startY; y < startY + rectHeight; y++)
            {
                grid[y, startX] = 1; // Borde izquierdo
                grid[y, startX + rectWidth - 1] = 1; // Borde derecho
                canvas.SetPixel(startX, y, boundaryColor);
                canvas.SetPixel(startX + rectWidth - 1, y, boundaryColor);
            }

            RedrawDisplay();
        }

        private void RedrawDisplay()
        {
            // Limpiar el bitmap de visualización
            displayGfx.Clear(bgColor);

            // Dibujar cada celda
            for (int y = 0; y < gridHeight; y++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    Rectangle cellRect = new Rectangle(x * cellSize, y * cellSize, cellSize, cellSize);

                    if (grid[y, x] == 1) // Límite
                    {
                        using (SolidBrush brush = new SolidBrush(boundaryColor))
                        {
                            displayGfx.FillRectangle(brush, cellRect);
                        }

                        if (chkShowNumbers.Checked)
                        {
                            DrawCellNumber(x, y, "B", Color.White);
                        }
                    }
                    else if (grid[y, x] == 2) // Rellenado
                    {
                        using (SolidBrush brush = new SolidBrush(fillColor))
                        {
                            displayGfx.FillRectangle(brush, cellRect);
                        }

                        if (chkShowNumbers.Checked)
                        {
                            DrawCellNumber(x, y, "S", Color.White);
                        }
                    }

                    // Dibujar borde de la celda si está activado
                    if (chkShowGrid.Checked)
                    {
                        using (Pen gridPen = new Pen(gridColor, 1))
                        {
                            displayGfx.DrawRectangle(gridPen, cellRect);
                        }
                    }
                }
            }

            picCanvas.Image = displayBitmap;
            picCanvas.Refresh();
        }

        private void ResetStats()
        {
            pixelsFilled = 0;
            neighborsExplored = 0;
            scanlinesProcessed = 0;
            UpdateStatsLabel();
        }

        private void UpdateStatsLabel()
        {
            if (lblStats.InvokeRequired)
            {
                lblStats.Invoke(new Action(UpdateStatsLabel));
                return;
            }
            lblStats.Text = $"Píxeles rellenados: {pixelsFilled}\nLíneas procesadas: {scanlinesProcessed}\nVecinos explorados: {neighborsExplored}";
        }

        private bool IsValidCell(int x, int y)
        {
            return x >= 0 && x < gridWidth && y >= 0 && y < gridHeight;
        }

        private Point FindRectangleCenter()
        {
            // Buscar los límites del rectángulo
            int minX = gridWidth, maxX = 0;
            int minY = gridHeight, maxY = 0;

            for (int y = 0; y < gridHeight; y++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    if (grid[y, x] == 1)
                    {
                        minX = Math.Min(minX, x);
                        maxX = Math.Max(maxX, x);
                        minY = Math.Min(minY, y);
                        maxY = Math.Max(maxY, y);
                    }
                }
            }

            // Calcular el centro
            int centerX = (minX + maxX) / 2;
            int centerY = (minY + maxY) / 2;

            return new Point(centerX, centerY);
        }

        private async void BtnStartFill_Click(object sender, EventArgs e)
        {
            // Verificar que hay un rectángulo dibujado
            bool hasRectangle = false;
            for (int y = 0; y < gridHeight && !hasRectangle; y++)
            {
                for (int x = 0; x < gridWidth && !hasRectangle; x++)
                {
                    if (grid[y, x] == 1) hasRectangle = true;
                }
            }

            if (!hasRectangle)
            {
                MessageBox.Show("Primero debes dibujar un rectángulo.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Encontrar el centro del rectángulo
            Point center = FindRectangleCenter();

            // Verificar que el centro no sea un límite
            if (grid[center.Y, center.X] == 1)
            {
                MessageBox.Show("No se pudo encontrar un punto interior válido.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnStartFill.Enabled = false;
            btnDrawRectangle.Enabled = false;
            btnStop.Enabled = true;
            numCellSize.Enabled = false;
            numRectWidth.Enabled = false;
            numRectHeight.Enabled = false;

            cancellationTokenSource = new CancellationTokenSource();
            ResetStats();
            isAnimating = true;

            try
            {
                int delay = GetDelayFromTrackbar();
                await CRelleno.ScanlineFillAnimated(
                    canvas,
                    center.X,
                    center.Y,
                    fillColor,
                    delay,
                    OnNeighborExplored,
                    OnPixelFilled,
                    cancellationTokenSource.Token
                );
            }
            catch (OperationCanceledException)
            {
                // Animación cancelada
            }
            finally
            {
                isAnimating = false;
                btnStartFill.Enabled = true;
                btnDrawRectangle.Enabled = true;
                btnStop.Enabled = false;
                numCellSize.Enabled = true;
                numRectWidth.Enabled = true;
                numRectHeight.Enabled = true;
                cancellationTokenSource?.Dispose();
                cancellationTokenSource = null;
                RedrawDisplay();
            }
        }

        private int lastScanlineY = -1;

        private async Task OnNeighborExplored(int fromX, int fromY, int toX, int toY, CRelleno.NeighborStatus status, CancellationToken token)
        {
            neighborsExplored++;

            // Detectar nueva línea de escaneo
            if (fromY != lastScanlineY && chkShowScanlines.Checked)
            {
                lastScanlineY = fromY;
                scanlinesProcessed++;
                DrawScanline(fromY);
                await Task.Delay(GetDelayFromTrackbar() / 2, token);
            }

            // Mostrar exploración horizontal (izquierda-derecha)
            if (chkShowExploration.Checked && fromY == toY)
            {
                Color arrowColor = status == CRelleno.NeighborStatus.Free
                    ? Color.FromArgb(3, 169, 244) // Azul claro
                    : Color.FromArgb(255, 193, 7); // Amarillo

                if (IsValidCell(fromX, fromY) && IsValidCell(toX, toY))
                {
                    DrawArrow(fromX, fromY, toX, toY, arrowColor);
                }
            }

            // Mostrar exploración vertical (arriba-abajo)
            if (chkShowBacktrack.Checked && fromX == toX && fromY != toY)
            {
                Color arrowColor = status == CRelleno.NeighborStatus.Free
                    ? Color.FromArgb(76, 175, 80) // Verde
                    : Color.FromArgb(244, 67, 54); // Rojo

                if (IsValidCell(fromX, fromY) && IsValidCell(toX, toY))
                {
                    DrawArrow(fromX, fromY, toX, toY, arrowColor);
                    await Task.Delay(GetDelayFromTrackbar() / 4, token);
                }
            }
        }

        private void DrawScanline(int y)
        {
            if (!IsValidCell(0, y)) return;

            using (Pen scanPen = new Pen(scanlineColor, 2))
            {
                int yPos = y * cellSize + cellSize / 2;
                displayGfx.DrawLine(scanPen, 0, yPos, gridWidth * cellSize, yPos);
            }

            picCanvas.Invalidate();
            picCanvas.Update();
            Application.DoEvents();
        }

        private async Task OnPixelFilled(int x, int y, CancellationToken token)
        {
            if (IsValidCell(x, y))
            {
                grid[y, x] = 2;
                pixelsFilled++;
                DrawCell(x, y, fillColor, chkShowNumbers.Checked);

                if (pixelsFilled % 5 == 0)
                {
                    UpdateStatsLabel();
                }
            }

            await Task.CompletedTask;
        }

        private void DrawCell(int gridX, int gridY, Color color, bool showNumber)
        {
            if (!IsValidCell(gridX, gridY)) return;

            Rectangle cellRect = new Rectangle(gridX * cellSize, gridY * cellSize, cellSize, cellSize);

            using (SolidBrush brush = new SolidBrush(color))
            {
                displayGfx.FillRectangle(brush, cellRect);
            }

            if (chkShowGrid.Checked)
            {
                using (Pen gridPen = new Pen(gridColor, 1))
                {
                    displayGfx.DrawRectangle(gridPen, cellRect);
                }
            }

            if (showNumber)
            {
                DrawCellNumber(gridX, gridY, "S", Color.White);
            }

            picCanvas.Invalidate(cellRect);
            picCanvas.Update();
            Application.DoEvents();
        }

        private void DrawCellNumber(int gridX, int gridY, string text, Color color)
        {
            Rectangle cellRect = new Rectangle(gridX * cellSize, gridY * cellSize, cellSize, cellSize);

            using (Font font = new Font("Consolas", Math.Max(6f, cellSize * 0.5f), FontStyle.Bold))
            using (SolidBrush textBrush = new SolidBrush(color))
            {
                var size = displayGfx.MeasureString(text, font);
                float tx = cellRect.X + (cellRect.Width - size.Width) / 2f;
                float ty = cellRect.Y + (cellRect.Height - size.Height) / 2f - 1f;
                displayGfx.DrawString(text, font, textBrush, tx, ty);
            }
        }

        private void DrawArrow(int fromX, int fromY, int toX, int toY, Color color)
        {
            float sx = fromX * cellSize + cellSize / 2f;
            float sy = fromY * cellSize + cellSize / 2f;
            float ex = toX * cellSize + cellSize / 2f;
            float ey = toY * cellSize + cellSize / 2f;

            float shrink = Math.Max(2f, cellSize * 0.2f);
            var vecX = ex - sx;
            var vecY = ey - sy;
            var len = (float)Math.Sqrt(vecX * vecX + vecY * vecY);
            if (len < 0.001f) return;

            var ux = vecX / len;
            var uy = vecY / len;
            var startX = sx + ux * shrink;
            var startY = sy + uy * shrink;
            var endX = ex - ux * shrink;
            var endY = ey - uy * shrink;

            using (var pen = new Pen(color, Math.Max(1f, cellSize * 0.12f)))
            {
                displayGfx.DrawLine(pen, startX, startY, endX, endY);
            }

            // Punta de flecha
            float arrowLen = Math.Max(6f, cellSize * 0.45f);
            float angle = (float)Math.Atan2(endY - startY, endX - startX);

            var p1 = new PointF(endX, endY);
            var p2 = new PointF(
                endX - arrowLen * (float)Math.Cos(angle - Math.PI / 6),
                endY - arrowLen * (float)Math.Sin(angle - Math.PI / 6));
            var p3 = new PointF(
                endX - arrowLen * (float)Math.Cos(angle + Math.PI / 6),
                endY - arrowLen * (float)Math.Sin(angle + Math.PI / 6));

            using (var br = new SolidBrush(color))
            {
                displayGfx.FillPolygon(br, new[] { p1, p2, p3 });
            }

            picCanvas.Invalidate();
            picCanvas.Update();
            Application.DoEvents();
        }

        private int GetDelayFromTrackbar()
        {
            int value = trkSpeed.Value;
            if (value <= 10) return 200;
            if (value <= 25) return 100;
            if (value <= 50) return 50;
            if (value <= 75) return 20;
            if (value <= 90) return 5;
            return 1;
        }

        private void BtnFillColor_Click(object sender, EventArgs e)
        {
            colorDialog.Color = fillColor;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                fillColor = colorDialog.Color;
                btnFillColor.BackColor = fillColor;
                btnFillColor.ForeColor = GetContrastColor(fillColor);
            }
        }

        private void BtnBackgroundColor_Click(object sender, EventArgs e)
        {
            colorDialog.Color = boundaryColor;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                boundaryColor = colorDialog.Color;
                btnBackgroundColor.BackColor = boundaryColor;
                btnBackgroundColor.ForeColor = GetContrastColor(boundaryColor);
            }
        }

        private Color GetContrastColor(Color color)
        {
            int luminance = (int)(0.299 * color.R + 0.587 * color.G + 0.114 * color.B);
            return luminance > 128 ? Color.Black : Color.White;
        }

        private void ChkAnimate_CheckedChanged(object sender, EventArgs e)
        {
            trkSpeed.Enabled = chkAnimate.Checked;
            lblSpeed.Enabled = chkAnimate.Checked;
            lblSpeedValue.Enabled = chkAnimate.Checked;
            grpVisualization.Enabled = chkAnimate.Checked;
        }

        private void TrkSpeed_ValueChanged(object sender, EventArgs e)
        {
            int value = trkSpeed.Value;
            if (value <= 10)
                lblSpeedValue.Text = "Muy Lenta";
            else if (value <= 25)
                lblSpeedValue.Text = "Lenta";
            else if (value <= 50)
                lblSpeedValue.Text = "Media";
            else if (value <= 75)
                lblSpeedValue.Text = "Rápida";
            else if (value <= 90)
                lblSpeedValue.Text = "Muy Rápida";
            else
                lblSpeedValue.Text = "Máxima";
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearCanvas();
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            cancellationTokenSource?.Cancel();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            cancellationTokenSource?.Cancel();
            gfx?.Dispose();
            displayGfx?.Dispose();
            canvas?.Dispose();
            displayBitmap?.Dispose();
            base.OnFormClosing(e);
        }

        private void ScanlineFillForm_Load(object sender, EventArgs e)
        {

        }
    }
}