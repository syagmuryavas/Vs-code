import pytest
from veritabani import Veritabani
from kutuphane import Kutuphane
from odunc import OduncYonetimi


def test_senaryo_1_kitap_ekleme_ve_listeleme():
    veritabani = Veritabani()
    kutuphane = Kutuphane(veritabani)

    kutuphane.kitap_ekle("1984", "George Orwell")

    kitaplar = kutuphane.tum_kitaplari_getir()

    assert len(kitaplar) == 1
    assert kitaplar[0].ad == "1984"


def test_senaryo_2_kitap_odunc_alma():
    veritabani = Veritabani()
    kutuphane = Kutuphane(veritabani)
    odunc = OduncYonetimi(kutuphane)

    kutuphane.kitap_ekle("Dune", "Frank Herbert")
    odunc.kitap_odunc_al("Dune")

    assert odunc.kitap_musait_mi("Dune") is False


def test_senaryo_3_kitap_iade_etme():
    veritabani = Veritabani()
    kutuphane = Kutuphane(veritabani)
    odunc = OduncYonetimi(kutuphane)

    kutuphane.kitap_ekle("Sefiller", "Victor Hugo")
    odunc.kitap_odunc_al("Sefiller")
    odunc.kitap_iade_et("Sefiller")

    assert odunc.kitap_musait_mi("Sefiller") is True


def test_senaryo_4_var_olmayan_kitap_odunc_alma_hatasi():
    veritabani = Veritabani()
    kutuphane = Kutuphane(veritabani)
    odunc = OduncYonetimi(kutuphane)

    with pytest.raises(ValueError):
        odunc.kitap_odunc_al("Bilinmeyen Kitap")