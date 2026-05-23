from veritabani import Veritabani
from kutuphane import Kutuphane
from odunc import OduncYonetimi


def test_kitap_ekleme_ve_odunc_alma_entegre_calisir():
    veritabani = Veritabani()
    kutuphane = Kutuphane(veritabani)
    odunc = OduncYonetimi(kutuphane)

    kutuphane.kitap_ekle("1984", "George Orwell")
    odunc.kitap_odunc_al("1984")

    assert odunc.kitap_musait_mi("1984") is False


def test_odunc_alma_ve_iade_etme_entegre_calisir():
    veritabani = Veritabani()
    kutuphane = Kutuphane(veritabani)
    odunc = OduncYonetimi(kutuphane)

    kutuphane.kitap_ekle("Dune", "Frank Herbert")
    odunc.kitap_odunc_al("Dune")
    odunc.kitap_iade_et("Dune")

    assert odunc.kitap_musait_mi("Dune") is True