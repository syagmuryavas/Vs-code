import pytest
from veritabani import Veritabani
from kutuphane import Kutuphane


def test_kutuphane_uzerinden_kitap_eklenebilmeli():
    veritabani = Veritabani()
    kutuphane = Kutuphane(veritabani)

    kutuphane.kitap_ekle("1984", "George Orwell")

    kitap = kutuphane.kitap_bul("1984")

    assert kitap is not None
    assert kitap.yazar == "George Orwell"


def test_kitap_adiyla_bulunabilmeli():
    veritabani = Veritabani()
    kutuphane = Kutuphane(veritabani)

    kutuphane.kitap_ekle("Dune", "Frank Herbert")

    bulunan = kutuphane.kitap_bul("Dune")

    assert bulunan is not None
    assert bulunan.ad == "Dune"


def test_ayni_kitap_iki_kez_eklenemez():
    veritabani = Veritabani()
    kutuphane = Kutuphane(veritabani)

    kutuphane.kitap_ekle("Sefiller", "Victor Hugo")

    with pytest.raises(ValueError):
        kutuphane.kitap_ekle("Sefiller", "Victor Hugo")


def test_musait_kitaplar_getirilebilmeli():
    veritabani = Veritabani()
    kutuphane = Kutuphane(veritabani)

    kutuphane.kitap_ekle("Suç ve Ceza", "Dostoyevski")

    musait_kitaplar = kutuphane.musait_kitaplari_getir()

    assert len(musait_kitaplar) == 1
    assert musait_kitaplar[0].ad == "Suç ve Ceza"
    
class FakeVeritabani:
    def __init__(self):
        self.eklenen_kitaplar = []

    def kitap_ekle(self, kitap):
        self.eklenen_kitaplar.append(kitap)

    def kitap_bul(self, kitap_adi):
        for kitap in self.eklenen_kitaplar:
            if kitap.ad == kitap_adi:
                return kitap
        return None

    def tum_kitaplari_getir(self):
        return self.eklenen_kitaplar

    def oduncteki_kitaplari_getir(self):
        return []

    def musait_kitaplari_getir(self):
        return self.eklenen_kitaplar


def test_fake_veritabani_ile_kitap_ekleme():
    fake_veritabani = FakeVeritabani()
    kutuphane = Kutuphane(fake_veritabani)

    kutuphane.kitap_ekle("Robot", "Isaac Asimov")

    assert len(fake_veritabani.eklenen_kitaplar) == 1
    assert fake_veritabani.eklenen_kitaplar[0].ad == "Robot"