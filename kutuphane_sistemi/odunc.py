class OduncYonetimi:
    def __init__(self, kutuphane):
        self.kutuphane = kutuphane

    def kitap_odunc_al(self, kitap_adi):
        kitap = self.kutuphane.kitap_bul(kitap_adi)

        if kitap is None:
            raise ValueError("Kitap bulunamadı.")

        kitap.odunc_ver()

    def kitap_iade_et(self, kitap_adi):
        kitap = self.kutuphane.kitap_bul(kitap_adi)

        if kitap is None:
            raise ValueError("Kitap bulunamadı.")

        kitap.iade_al()

    def kitap_musait_mi(self, kitap_adi):
        kitap = self.kutuphane.kitap_bul(kitap_adi)

        if kitap is None:
            raise ValueError("Kitap bulunamadı.")

        return kitap.musait_mi()