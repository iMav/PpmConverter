﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PpmConverter.Jpeg
{
    public class JpegEncoder
    {
        private Bitstream bitstream;

        public JpegEncoder()
        {
            bitstream = new Bitstream();
        }

        public void SaveIntoFile(string path)
        {
            bitstream.FlushIntoFile(path);
        }

        public void WriteMarker(PPMImage image)
        {
            WriteApp0();
            WriteSof0(image);
        }

        private void WriteApp0()
        {
            //Marker
            bitstream.WriteByte(0xff);
            bitstream.WriteByte(0xe0);

            //Laenge des Segments
            bitstream.WriteByte(0x00);
            bitstream.WriteByte(0x10);

            //JFIF 0x00
            bitstream.WriteByte(0x4a);
            bitstream.WriteByte(0x46);
            bitstream.WriteByte(0x49);
            bitstream.WriteByte(0x46);
            bitstream.WriteByte(0x00);

            //Major revision number 1
            bitstream.WriteByte(0x01);

            //Minor revision number 0..2 hier: 1
            bitstream.WriteByte(0x01);

            //Einheit der Pixelgroesse 0=Keine Einheit, sondern Seitenverhältnis
            bitstream.WriteByte(0x00);

            //x-Dichte != 0 hier: 0x0048
            bitstream.WriteByte(0x00);
            bitstream.WriteByte(0x48);

            //y-Dichte != 0 hier: 0x0048
            bitstream.WriteByte(0x00);
            bitstream.WriteByte(0x48);

            //groesse x y vorschaubild 0 = kein Vorschaubild
            bitstream.WriteByte(0x00);
            bitstream.WriteByte(0x00);

            //n-bytes fuer vorschaubild (x*y*3); Für keine Vorschau, kein byte
            
        }

        private void WriteSof0(PPMImage image)
        {
            //Marker
            bitstream.WriteByte(0xff);
            bitstream.WriteByte(0xc0);

            //Laenge: 8 + anz. Komponenten * 3
            //JFIF benutzt entweder 1 Komponente (Y, Grauwertbilder) oder 3 Komponenten(YCbCr, Farbbilder)
            byte b = 8 + 3 * 3;
            bitstream.WriteByte(0x00);
            bitstream.WriteByte(b);

            //Genauigkeit der Daten in bits/sample: normal=8 (12 u. 16 meist nicht unterstützt)
            bitstream.WriteByte(0x08);

            //Bildgroesse y > 0
            
            bitstream.WriteByte((byte)(image.Matrix.R.GetLength(1) >> 8));
            bitstream.WriteByte((byte)image.Matrix.R.GetLength(1));

            //Bildgroesse x > 0
            bitstream.WriteByte((byte)(image.Matrix.R.GetLength(0) >> 8));
            bitstream.WriteByte((byte)image.Matrix.R.GetLength(0));

            //Anzahl Komponenten
            bitstream.WriteByte(0x03);

            //Komponente Y
            //  ID=1
            bitstream.WriteByte(0x01);
            //  Faktor unterabtastung (Bit 0-3 vertikal, 4-7 Horizontal);  Keine Unterabtastung: 0x22, Unterabtastung Faktor 2: 0x11
            bitstream.WriteByte(0x11);
            //  Nummer der Quantisierungstabelle [KEIN PLAN]
            bitstream.WriteByte(0x01);

            //Komponente Cb
            //  ID=2
            bitstream.WriteByte(0x02);
            //  Faktor unterabtastung (Bit 0-3 vertikal, 4-7 Horizontal);  Keine Unterabtastung: 0x22, Unterabtastung Faktor 2: 0x11
            bitstream.WriteByte(0x11);
            //  Nummer der Quantisierungstabelle [KEIN PLAN]
            bitstream.WriteByte(0x01);

            //Komponente Cr
            //  ID=2
            bitstream.WriteByte(0x03);
            //  Faktor unterabtastung (Bit 0-3 vertikal, 4-7 Horizontal);  Keine Unterabtastung: 0x22, Unterabtastung Faktor 2: 0x11
            bitstream.WriteByte(0x11);
            //  Nummer der Quantisierungstabelle [KEIN PLAN]
            bitstream.WriteByte(0x01);
        }
    }
}
