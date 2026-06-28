import type { Metadata } from "next";
import { Geist_Mono, Inter, Outfit } from "next/font/google";

import { GoogleAuthProvider } from "@/providers/google-auth-provider";
import { QueryProvider } from "@/providers/query-provider";

import "./globals.css";

const inter = Inter({
  variable: "--font-inter",
  subsets: ["latin", "latin-ext"],
  display: "swap",
});

const outfit = Outfit({
  variable: "--font-outfit",
  subsets: ["latin", "latin-ext"],
  display: "swap",
});

const geistMono = Geist_Mono({
  variable: "--font-geist-mono",
  subsets: ["latin"],
});

export const metadata: Metadata = {
  title: "SmartCashback | Nền tảng hoàn tiền thông minh",
  description: "Nền tảng hoàn tiền và liên kết tiếp thị cho người tiêu dùng Việt Nam",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="vi">
      <body
        className={`${inter.variable} ${outfit.variable} ${geistMono.variable} antialiased`}
      >
        <QueryProvider>
          <GoogleAuthProvider>{children}</GoogleAuthProvider>
        </QueryProvider>
      </body>
    </html>
  );
}
