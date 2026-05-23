class Veritabani:
    def __init__(self):
        self.kitaplar = []

    def kitap_ekle(self, kitap):
        if self.kitap_bul(kitap.ad) is not None:
            raise ValueError("Bu kitap zaten mevcut.")
        self.kitaplar.append(kitap)

    def kitap_bul(self, kitap_adi):
        for kitap in self.kitaplar:
            if kitap.ad == kitap_adi:
                return kitap
        return None

    def tum_kitaplari_getir(self):
        return self.kitaplar

    def oduncteki_kitaplari_getir(self):
        return [kitap for kitap in self.kitaplar if kitap.durum == "oduncte"]

    def musait_kitaplari_getir(self):
        return [kitap for kitap in self.kitaplar if kitap.durum == "musait"]