import pytest
from kitap import Kitap
from veritabani import Veritabani


def test_kitap_eklenebilmeli():
    veritabani = Veritabani()
    kitap = Kitap("1984", "George Orwell")

    veritabani.kitap_ekle(kitap)

    assert len(veritabani.tum_kitaplari_getir()) == 1


def test_ayni_kitap_iki_kez_eklenemez():
    veritabani = Veritabani()
    kitap1 = Kitap("Dune", "Frank Herbert")
    kitap2 = Kitap("Dune", "Başka Yazar")

    veritabani.kitap_ekle(kitap1)

    with pytest.raises(ValueError):
        veritabani.kitap_ekle(kitap2)


def test_kitap_adiyla_bulunabilmeli():
    veritabani = Veritabani()
    kitap = Kitap("Sefiller", "Victor Hugo")

    veritabani.kitap_ekle(kitap)

    bulunan = veritabani.kitap_bul("Sefiller")

    assert bulunan == kitap


def test_musait_kitaplar_listelenebilmeli():
    veritabani = Veritabani()
    kitap = Kitap("Suç ve Ceza", "Dostoyevski")

    veritabani.kitap_ekle(kitap)

    musait_kitaplar = veritabani.musait_kitaplari_getir()

    assert len(musait_kitaplar) == 1
    assert musait_kitaplar[0].ad == "Suç ve Ceza"


def test_oduncteki_kitaplar_listelenebilmeli():
    veritabani = Veritabani()
    kitap = Kitap("Yüzüklerin Efendisi", "Tolkien")

    kitap.odunc_ver()
    veritabani.kitap_ekle(kitap)

    oduncteki_kitaplar = veritabani.oduncteki_kitaplari_getir()

    assert len(oduncteki_kitaplar) == 1
    assert oduncteki_kitaplar[0].ad == "Yüzüklerin Efendisi"