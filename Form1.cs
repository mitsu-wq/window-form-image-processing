namespace lab_12;

public partial class Form1 : Form
{
    Button button1 = new Button(), button2  = new Button(), button3 = new Button();
    PictureBox pictureBox1 = new PictureBox();
    TextBox textBox1 = new TextBox(), textBox2 = new TextBox();
    private Pen blackPen;
    private Bitmap bmp;
    private Graphics g;
    Color pencolor = Color.Black;
    int pensize = 4;
    private Point PreviousPoint, point;
    public Form1()
    {
        InitializeComponent();

        button1.Text = "Открыть";
        button2.Text = "Сохранить";
        button3.Text = "Серый";
        textBox1.Text = "Введите цвет кисти";
        textBox2.Text = "Введите размер кисти";

        button1.Location = new Point(100, 400);
        button2.Location = new Point(500, 400);
        button3.Location = new Point(300, 400);
        textBox1.Location = new Point(200, 400);
        textBox2.Location = new Point(400, 400);

        this.Load += this.Form1_Load;
        button1.Click += this.button1_Click;
        button2.Click += this.button2_Click;
        button3.Click += this.button3_Click;
        pictureBox1.MouseDown += this.pictureBox1_MouseDown;
        pictureBox1.MouseMove += this.pictureBox1_MouseMove;
        textBox1.TextChanged += this.textBox1_TextChanged;
        textBox2.TextChanged += this.textBox2_TextChanged;
    
        this.Controls.Add(button1);
        this.Controls.Add(button2);
        this.Controls.Add(button3);
        this.Controls.Add(textBox1);
        this.Controls.Add(textBox2);
        this.Controls.Add(pictureBox1);
    }
    private void textBox1_TextChanged(object sender, EventArgs e){
        pencolor = Color.FromName(textBox1.Text);
        blackPen = new Pen(pencolor, pensize);
    }
    private void textBox2_TextChanged(object sender, EventArgs e){
        try{
            pensize = int.Parse(textBox2.Text);
            blackPen = new Pen(pencolor, pensize);
        } catch { }
    }
    private void Form1_Load(object sender, EventArgs e){
        blackPen = new Pen(pencolor, pensize);
    }
    private void button1_Click(object sender, EventArgs e){
        OpenFileDialog dialog = new OpenFileDialog();
        dialog.Filter = "Image files (*.BMP, *.JPG, *.GIF, *.PNG)|*.bmp;*.jpg;*.gif;*.png";
        if (dialog.ShowDialog() == DialogResult.OK){
            Image image = Image.FromFile(dialog.FileName);
            int width = image.Width;
            int height = image.Height;
            pictureBox1.Width = width;
            pictureBox1.Height = height;
            bmp = new Bitmap(image, width, height);
            pictureBox1.Image = bmp;
            g = Graphics.FromImage(pictureBox1.Image);
        }
    }
    private void pictureBox1_MouseDown(object sender, MouseEventArgs e){
        PreviousPoint.X = e.X;
        PreviousPoint.Y = e.Y;
    }
    private void pictureBox1_MouseMove(object sender, MouseEventArgs e){
        if (e.Button == MouseButtons.Left){
            point.X = e.X;
            point.Y = e.Y;
            g.DrawLine(blackPen, PreviousPoint, point);
            PreviousPoint.X = point.X;
            PreviousPoint.Y = point.Y;
            pictureBox1.Invalidate();
        }
    }
    private void button2_Click(object sender, EventArgs e){
        SaveFileDialog savedialog = new SaveFileDialog();
        savedialog.Title = "Сохранить картинку как ...";
        savedialog.OverwritePrompt = true;
        savedialog.CheckPathExists = true;
        savedialog.Filter = "Bitmap File(*.bmp)|*.bmp|" + "GIF File(*.gif)|*.gif|" + "JPEG File(*.jpg)|*.jpg|" + "PNG File(*.png)|*.png";
        if (savedialog.ShowDialog() == DialogResult.OK){
            string fileName = savedialog.FileName;
            string strFilExtn = fileName.Remove(0, fileName.Length - 3);
            switch(strFilExtn){
                case "bmp":
                    bmp.Save(fileName, System.Drawing.Imaging.ImageFormat.Bmp);
                    break;
                case "jpg":
                    bmp.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    break;
                case "gif":
                    bmp.Save(fileName, System.Drawing.Imaging.ImageFormat.Gif);
                    break;
                case "tif":
                    bmp.Save(fileName, System.Drawing.Imaging.ImageFormat.Tiff);
                    break;
                case "png":
                    bmp.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
                    break;
                default:
                    break;
            }
        }
    }
    private void button3_Click(object sender, EventArgs e){
        for (int i = 0; i < bmp.Width; i++){
            for (int j = 0; j < bmp.Height; j++){
                int R = bmp.GetPixel(i, j).R;
                int G = bmp.GetPixel(i, j).G;
                int B = bmp.GetPixel(i, j).B;
                int Gray = (R + G + B) / 3;
                Color p = Color.FromArgb(255, Gray, Gray, Gray);
                bmp.SetPixel(i, j, p);
            }
        }
        Refresh();
    }
}
