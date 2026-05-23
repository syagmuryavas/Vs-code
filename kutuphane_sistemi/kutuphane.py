from kitap import Kitap


class Kutuphane:
    def __init__(self, veritabani):
        self.veritabani = veritabani

    def kitap_ekle(self, ad, yazar):
        kitap = Kitap(ad, yazar)
        self.veritabani.kitap_ekle(kitap)

    def kitap_bul(self, kitap_adi):
        return self.veritabani.kitap_bul(kitap_adi)

    def tum_kitaplari_getir(self):
        return self.veritabani.tum_kitaplari_getir()

    def oduncteki_kitaplari_getir(self):
        return self.veritabani.oduncteki_kitaplari_getir()

    def musait_kitaplari_getir(self):
        return self.veritabani.musait_kitaplari_getir()