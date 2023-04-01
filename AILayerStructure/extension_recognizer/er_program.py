import os
import json
import numpy as np
from sklearn import tree
from skl2onnx import convert_sklearn
from skl2onnx.common.data_types import Int16TensorType, FloatTensorType


root_folder = "./../WebApp.Shop/wwwroot/ai_structure/"


def load_data():
    with open(root_folder + "inputs/file_structures.json") as file_data:
        js_data = json.load(file_data)
        x_data = np.zeros(shape=(0, 50), dtype="int16")
        y_data = np.zeros(shape=(0, 1))
        for row in js_data:
            for hex_signature in row["hex_signatures"]:
                hex_signature = [int(i, 16) for i in hex_signature]
                if len(hex_signature) < 50:
                    count = 50 - len(hex_signature)
                    for i in range(count):
                        hex_signature.append(0)
                x_data = np.vstack(
                    (x_data, np.array(hex_signature, "int16").reshape(1, 50))
                )
                y_data = np.append(y_data, row["id"])
        return x_data, y_data.reshape(-1, 1)


def compile_model():
    x_data, y_data = load_data()
    clf = tree.DecisionTreeClassifier()
    clf = clf.fit(x_data, y_data)
    initial_type = [("X", FloatTensorType([None, x_data.shape[1]]))]
    onx = convert_sklearn(clf, initial_types=initial_type)
    with open(root_folder + "outputs/rf_iris.onnx", "wb") as f:
        f.write(onx.SerializeToString())
