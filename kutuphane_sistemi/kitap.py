class Kitap:
    def __init__(self, ad, yazar):
        self.ad = ad
        self.yazar = yazar
        self.durum = "musait"

    def musait_mi(self):
        return self.durum == "musait"

    def odunc_ver(self):
        if not self.musait_mi():
            raise ValueError("Kitap zaten ödünçte.")
        self.durum = "oduncte"

    def iade_al(self):
        self.durum = "musait"

    def __repr__(self):
        return f"Kitap(ad='{self.ad}', yazar='{self.yazar}', durum='{self.durum}')"