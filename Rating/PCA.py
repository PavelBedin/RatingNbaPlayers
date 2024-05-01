import matplotlib.pyplot as plt
import numpy as np
from sklearn.decomposition import PCA


class PCA_builder():
    def __init__(self):
        self.data = None
        self.data_x = None
        self.data_y = None

    def data_processing(self, data_trad_stat, data_adv_stat):
        data_trad_stat = np.array(data_trad_stat)
        data_adv_stat = np.array(data_adv_stat)
        return np.concatenate([data_trad_stat, data_adv_stat], axis=1)

    def normalization(self, data):
        data_nor = np.array(data)
        std_data = np.std(data_nor)
        mean_val = np.mean(data_nor)
        return (data_nor - mean_val) / std_data

    def make_PCA(self, data_trad_stat: list, data_adv_stat: list, n_components: int):
        length = len(data_trad_stat)
        self.data = self.data_processing(data_trad_stat, data_adv_stat)
        self.data.reshape((length, 2, -1)).mean(axis=1)
        pca = PCA(n_components=n_components)
        transformed_data = pca.fit_transform(self.data)
        result = []
        for i in range(n_components):
            if len(result) == 0:
                result = self.normalization([row[0] for row in transformed_data])
            else:
                result = list(zip(result, self.normalization([row[i] for row in transformed_data])))
        return result

    def show_new_component(self, x: list, y: list):
        plt.scatter(x, y, color='blue', marker='o')
        count = 1
        for i, txt in enumerate(y):
            plt.text(x[i], y[i], f'{count}', ha='left')
            count += 1

        plt.show()
